using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private bool _shouldFaceMoveDirection = false;

    private CharacterController _controller;
    private Vector3 _moveInput;
    private Vector3 _Velocity;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {_moveInput}");
    }

    private void Update()
    {
        Vector3 foward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        foward.y = 0;
        right.y = 0;

        foward.Normalize();
        right.Normalize();

        Vector3 moveDirection = foward * _moveInput.y + right * _moveInput.x;
        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);

        if (_shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        } 
    }
}
