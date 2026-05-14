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
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashDuaration = 0.25f;
    [SerializeField] private float _dashCooldown = 0.5f;

    [SerializeField] private Animator _animator;

    public GameEvent Ping;
    private bool _canDash = true;
    private bool _active;


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
        if (!_active) return;
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
        if (_active)
        {
            Vector3 foward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;

            foward.y = 0;
            right.y = 0;

            foward.Normalize();
            right.Normalize();

            _moveDirection = foward * _moveInput.y + right * _moveInput.x;
            _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);

            if (_moveDirection != Vector3.zero)
            {
                _animator.SetBool("Movement", true);
            }
            else
            {
                _animator.SetBool("Movement", false);
            }
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
        if (data is not TurnManager.CurrentTurn.PlayerTurn)
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
}
