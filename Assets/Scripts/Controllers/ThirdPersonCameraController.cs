using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private float _zoomSpeed = 2f;
    //the smoothness of the zooming
    [SerializeField] private float _zoomLerpSpeed = 10f;
    //how close the camera can get to the player
    [SerializeField] private float _minimumZoomDistance = 3f;
    //how far the camera can get from the player
    [SerializeField] private float _maximumZoomDistance = 15f;

    private PlayerControls _controls;
    private CinemachineCamera _cinemachineCamera;
    private CinemachineOrbitalFollow _cinemachineOrbital;
    private Vector2 _scrollDelta;

    private float _targetZoom;
    private float _currentZoom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controls = new PlayerControls();
        _controls.Enable();
        _controls.CameraControls.MouseZoom.performed += HandleMouseScroll;

        Cursor.lockState = CursorLockMode.Locked;

        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _cinemachineOrbital = _cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();

        _targetZoom = _currentZoom = _cinemachineOrbital.Radius;
    }

    private void HandleMouseScroll(InputAction.CallbackContext context)
    {
        _scrollDelta = context.ReadValue<Vector2>();
        Debug.Log($"Mouse Scroll Delta: {_scrollDelta}");
    }

    // Update is called once per frame
    void Update()
    {
        if (_scrollDelta.y != 0)
        {
            if (_cinemachineOrbital != null)
            {
                _targetZoom = Mathf.Clamp(_cinemachineOrbital.Radius - _scrollDelta.y * _zoomSpeed, _minimumZoomDistance, _maximumZoomDistance);
                _scrollDelta = Vector2.zero; // Reset scroll delta after processing
            }
        }

        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.deltaTime * _zoomLerpSpeed);
        _cinemachineOrbital.Radius = _currentZoom;
    }
}
