using System;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    
    
    void Start()
    {
        Bomb(Attack.AreaType.Line);
    }

    public void Bomb(Attack attack)
    {
        GameObject buffer;
        
    }
    
    void Bomb(Attack.AreaType areaType)
    {
        switch (areaType)
        {
            case Attack.AreaType.Line:
                Instantiate(prefabline, transform.position + cam.forward * spawnOffset, cam.rotation, transform);
                break;
            case Attack.AreaType.Cone:
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
