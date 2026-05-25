using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveAcceleration = 10f;
    [SerializeField] private float _moveDeceleration = 15f;
    [SerializeField] private float _rotationSpeed = 10f;
    private Vector3 _currentVelocity;

    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashDuaration = 0.25f;
    [SerializeField] private float _dashCooldown = 0.5f;

    [SerializeField] private Animator _animator;
    private float _velocityZ;
    private float _velocityX;

    public GameEvent Ping;
    private bool _canDash = true;
    private bool _active;
    private bool _isStunned = false;

    private CharacterController _controller;
    private Vector3 _moveInput;
    private Vector3 _moveDirection;
    private Vector3 _cameraForward;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {_moveInput}");
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!_active || _isStunned == true) return;
        if (context.performed && _canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnEndTurn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ping.Raise(this, null);
        }
    }
    private void Update()
    {
        //player movement only works during the enemy turn
        if (_active && !_isStunned)
        {
            Vector3 foward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;

            foward.y = 0;
            right.y = 0;

            foward.Normalize();
            right.Normalize();

            _moveDirection = foward * _moveInput.y + right * _moveInput.x;

            Vector3 targetVelocity = _moveDirection * _moveSpeed;

            float smoothRate = _moveDirection.magnitude > 0.1f ? _moveAcceleration : _moveDeceleration;

            _currentVelocity = Vector3.MoveTowards(_currentVelocity, targetVelocity, smoothRate * Time.deltaTime);

            _controller.Move(_currentVelocity * Time.deltaTime);

            _velocityZ = Mathf.Lerp(_velocityZ, _moveInput.y, Time.deltaTime * 10f);
            _velocityX = Mathf.Lerp(_velocityX, _moveInput.x, Time.deltaTime * 10f);

            _animator.SetFloat("VelocityZ", _velocityZ);
            _animator.SetFloat("VelocityX", _velocityX);

            _animator.SetBool("Movement", _moveInput.sqrMagnitude > 0.01f);
        }
        else
        {
            _animator.SetFloat("VelocityZ", 0f);
            _animator.SetFloat("VelocityX", 0f);
            _animator.SetBool("Movement", false);
        }

        _cameraForward = _cameraTransform.forward;
        _cameraForward.y = 0f;
        if (_cameraForward.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(_cameraForward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }

        if (transform.position.y != 1f)
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _animator.SetTrigger("Dash");
        float startTime = Time.time;

        while (Time.time < startTime + _dashDuaration)
        {
            _controller.Move(_moveDirection * _dashSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(DashCooldown());
    }

    public void ActivatePlayer(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.EnemyTurn)
        {
            _active = false;
            return;
        }
        _active = true;

    }
    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldown);
        _animator.ResetTrigger("Dash");
        _canDash = true;
    }

    public void StunPlayer(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        _isStunned = true;

        // stop movement immediately
        _currentVelocity = Vector3.zero;
        _moveInput = Vector2.zero;

        _animator.SetBool("Movement", false);

        yield return new WaitForSeconds(duration);

        _isStunned = false;
    }
}
