using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private bool _shouldFaceMoveDirection = false;

    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuaration;
    [SerializeField] private float _dashCooldown;

    private bool _canDash = true;


    private CharacterController _controller;
    private Vector3 _moveInput;
    private Vector3 _Velocity;
    private Vector3 _moveDirection;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {_moveInput}");
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && _canDash == true)
        {
            StartCoroutine(Dash());
        }
    }

    private void Update()
    {
        Vector3 foward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        foward.y = 0;
        right.y = 0;

        foward.Normalize();
        right.Normalize();

        _moveDirection = foward * _moveInput.y + right * _moveInput.x;
        _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);

        if (_shouldFaceMoveDirection && _moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    private IEnumerator Dash()
    {
        _canDash = false;

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
        _canDash = true;
    }
}
