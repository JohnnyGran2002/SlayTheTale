using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class Bomber : MonoBehaviour
{
    [SerializeField] private float spawnOffset;
    [SerializeField] private Transform cam;
    
    [SerializeField] private GameObject prefabline;
    [SerializeField] private GameObject prefabcone;
    [SerializeField] private GameObject prefabcircle;
    
    public enum Typ{
        Line,
        Circle,
        Cone
    }
    void Start()
    {
        Bomb(Typ.Line);
    }

    void Bomb(Typ areaType)
    {
        switch (areaType)
        {
            case Bomber.Typ.Line:
                Instantiate(prefabline, transform.position + cam.forward * spawnOffset, cam.rotation, transform);
                break;
            case Bomber.Typ.Circle:
                // code block
                break;
            default:
                // code block
                break;
        }
    }
    
    
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.pink;
        Gizmos.DrawLine(pos, pos + cam.forward * spawnOffset);
        
    }
    
}
