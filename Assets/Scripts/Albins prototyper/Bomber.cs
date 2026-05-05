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
    private Transform cam;

    [SerializeField] private GameObject prefabline;
    [SerializeField] private GameObject prefabcone;
    [SerializeField] private GameObject prefabcircle;


    void Start()
    {
        cam = Camera.main.transform;
    }

    private void Bomb(Attack attack)
    {
        GameObject buffer;
    }

    public void LineBomb()
    {
        Bomb(Attack.AreaType.Line);
    }

    public void BoxBomb()
    {
        Bomb(Attack.AreaType.Cone);
    }

    public void Bomb(Attack.AreaType areaType)
    {
        switch (areaType)
        {
            case Attack.AreaType.Line:
                Instantiate(prefabline, transform.position + Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized * spawnOffset, Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.forward, Vector3.up)), transform);
                break;
            case Attack.AreaType.Cone:
                Instantiate(prefabcone, transform.position + Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized * spawnOffset, Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.forward, Vector3.up)), transform);
                break;
            case Attack.AreaType.Circle:
                Instantiate(prefabcircle, transform.position + Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized * spawnOffset, Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.forward, Vector3.up)), transform);
                break;
            default:
                Debug.LogException(new Exception("how did you even?"), this);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.pink;
        //Gizmos.DrawLine(pos, pos + cam.forward * spawnOffset);
    }

}
