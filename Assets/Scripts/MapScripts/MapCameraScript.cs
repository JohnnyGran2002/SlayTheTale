using System;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MapCameraScript : MonoBehaviour
{
    [Header("Dependencies"), SerializeField, Tooltip("Empty Object")] private Transform target;
    
    
    [Header("Settings"), Space(7), SerializeField] private float scrollSpeed = 400f;
    [SerializeField] private float zoomSmoothing = 10f; 
    
    private Vector2 _scrollInput;
    [HideInInspector] public Vector3 startPos;

    private void Start()
    {
        //Sets the camera at the middle of the map
        transform.position = startPos;
        target.position = startPos;
    }

    public void OnScrollWheel(InputValue value)
    {
        _scrollInput = value.Get<Vector2>();
    }

    private void Update()
    {
        //Moves the "target" forward, the camera follows this object.
        var moveDirection = _scrollInput.y;

        if (Mathf.Abs(moveDirection) > 0.01f)
        {
            Vector3 forwardMove = target.forward;
            forwardMove.y = 0; 
            forwardMove.Normalize();
            
            target.transform.Translate(forwardMove * moveDirection * scrollSpeed * Time.deltaTime, Space.World);
        }
        
        //Makes the camera follow the target
        transform.position = Vector3.Lerp(transform.position, target.position, zoomSmoothing * Time.deltaTime);
    }
}