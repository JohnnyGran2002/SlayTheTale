using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class Bomber : MonoBehaviour
{
    private Vector3 spawnPos;

    [SerializeField] private GameObject prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Bomb()
    {
        
        
    }
    /*
    private Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.pink;
        Gizmos.DrawLine(Vector3.zero, Vector3.ProjectOnPlane(GetMousePos(), Vector3.up));
        Gizmos.DrawLine(Vector3.zero, GetMousePos());
    }
    */
}
