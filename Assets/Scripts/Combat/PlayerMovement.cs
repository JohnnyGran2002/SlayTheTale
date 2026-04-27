using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    private Vector3 _moveDirection;

    [SerializeField] private InputActionReference _moveAction;

    private void Update()
    {
        _moveDirection = _moveAction.action.ReadValue<Vector3>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_moveDirection * _moveSpeed, ForceMode.Impulse);
        //transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
        
    }
}
