using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private float spawnOffset;
    private Transform cam;
    private Statmanager statManager;
    [SerializeField] private GameObject SpellEffactPrefab;
    [SerializeField] private GameObject prefabline;
    [SerializeField] private GameObject prefabcone;
    [SerializeField] private GameObject prefabcircle;


    void Start()
    {
        cam = Camera.main.transform;
        statManager = GetComponentInParent<Statmanager>();
        if (statManager = null)
        {
            throw new Exception("no statmanager found on player");
        }
    }

    private void Bomb(Component sender, object data)
    {
        GameObject buffer;
        Attack bufferAttack;
        SpellEffect effect;
        if (data is Attack)
        {
            bufferAttack = (Attack)data;

            buffer = Instantiate(SpellEffactPrefab, transform.position + Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized * spawnOffset, Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.forward, Vector3.up)), transform);
            effect = buffer.GetComponent<SpellEffect>();
            Attack.InsertValues(bufferAttack, effect);
            
        }
    }

    
    

    public void CastSpell(Component sender, object data)
    {
        if (data is Attack.AreaType)
        {
            Attack.AreaType areaType = (Attack.AreaType)data;

            switch (areaType)
            {
                case Attack.AreaType.Square:
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
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.pink;
        //Gizmos.DrawLine(pos, pos + cam.forward * spawnOffset);
    }

}
