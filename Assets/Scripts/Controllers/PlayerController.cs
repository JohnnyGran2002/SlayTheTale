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
    [SerializeField] private bool _shouldFaceMoveDirection = false;

    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashDuaration = 0.25f;
    [SerializeField] private float _dashCooldown = 0.5f;
    
    [SerializeField] private Animator _animator;

    private bool _canDash = true;


    private CharacterController _controller;
    private Vector3 _moveInput;
    private Vector3 _moveDirection;

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
        if (context.performed && _canDash == true && TurnManager.Instance.currentTurnStatus == TurnManager.turnStatus.enemyTurn)
        {
            StartCoroutine(Dash());
        }
    }

    private void Update()
    {
        if (TurnManager.Instance.currentTurnStatus == TurnManager.turnStatus.enemyTurn)
        {
            Vector3 foward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;

            foward.y = 0;
            right.y = 0;

            foward.Normalize();
            right.Normalize();

            _moveDirection = foward * _moveInput.y + right * _moveInput.x;
            _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);

        }
        
        if (_moveDirection != Vector3.zero)
        {
            _animator.SetBool("Movement", true);
        }
        else
            _animator.SetBool("Movement", false);
        if (_shouldFaceMoveDirection && _moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
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

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldown);
        _animator.ResetTrigger("Dash");
        _canDash = true;
    }
}
