using System;
using System.Collections;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private float _playerTurnZoom;
    [SerializeField] private float _enemyTurnZoom;
    [SerializeField] private Vector3 _playerTurnOffset;
    [SerializeField] private Vector3 _enemyTurnOffset;
    //the smoothness of the zooming
    [SerializeField] private float _zoomLerpSpeed = 1f;
    [SerializeField] private float _offsetLerpSpeed = 1f;

    private CinemachineCamera _cinemachineCamera;
    private CinemachineOrbitalFollow _cinemachineOrbital;
    private CinemachineCameraOffset _cinemachineCameraOffset;

    private float _currentZoom;
    private Vector3 _currentOffset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _cinemachineOrbital = _cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
        _cinemachineCameraOffset = _cinemachineCamera.GetComponent<CinemachineCameraOffset>();

        _currentZoom = _cinemachineOrbital.Radius;
        _currentOffset = _cinemachineCameraOffset.Offset;
    }

    public void SetPlayerTurnCameraEvent()
    {
        StartCoroutine(PlayerTurnCamera());
    }

    public void SetEnemyTurnCameraEvent()
    {
        StartCoroutine(EnemyTurnCamera());
    }

    private IEnumerator EnemyTurnCamera()
    {
        //the treshold for snapping to the target zoom and offset, to avoid infinite small movements
        float zoomEpsilon = 0.01f;
        float offsetEpsilon = 0.01f;
        while (true)
        {
            // update toward target
            _currentZoom = Mathf.MoveTowards(_currentZoom, _enemyTurnZoom, Time.deltaTime * _zoomLerpSpeed);
            _cinemachineOrbital.Radius = _currentZoom;
            _currentOffset = Vector3.MoveTowards(_currentOffset, _enemyTurnOffset, Time.deltaTime * _offsetLerpSpeed);
            _cinemachineCameraOffset.Offset = _currentOffset;

            if (Mathf.Abs(_currentZoom - _enemyTurnZoom) <= zoomEpsilon && Vector3.Distance(_currentOffset, _enemyTurnOffset) <= offsetEpsilon)
            {
                // snap exactly
                _currentZoom = _enemyTurnZoom;
                _cinemachineOrbital.Radius = _enemyTurnZoom;
                _currentOffset = _enemyTurnOffset;
                _cinemachineCameraOffset.Offset = _enemyTurnOffset;
                break;
            }

            yield return null;
        }
    }

    private IEnumerator PlayerTurnCamera()
    {

        float zoomEpsilon = 0.01f;
        float offsetEpsilon = 0.01f;
        while (true)
        {
            _currentZoom = Mathf.MoveTowards(_currentZoom, _playerTurnZoom, Time.deltaTime * _zoomLerpSpeed);
            _cinemachineOrbital.Radius = _currentZoom;
            _currentOffset = Vector3.MoveTowards(_currentOffset, _playerTurnOffset, Time.deltaTime * _offsetLerpSpeed);
            _cinemachineCameraOffset.Offset = _currentOffset;

            if (Mathf.Abs(_currentZoom - _playerTurnZoom) <= zoomEpsilon && Vector3.Distance(_currentOffset, _playerTurnOffset) <= offsetEpsilon)
            {
                _currentZoom = _playerTurnZoom;
                _cinemachineOrbital.Radius = _playerTurnZoom;
                _currentOffset = _playerTurnOffset;
                _cinemachineCameraOffset.Offset = _playerTurnOffset;
                break;
            }

            yield return null;
        }
    }
}
