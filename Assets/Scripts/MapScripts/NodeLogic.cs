using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeLogic : MonoBehaviour, IPointerClickHandler
{
    //Generation
    public bool assigned, used;
    public List<GameObject> nextNode = new List<GameObject>();
    
    public enum Type
    {
        None,
        Combat,
        Elite,
        Event,
        Shop,
        Rest,
        Treasure,
        Boss,
    }
    public Type type;

    public EnemyData enemyData;

    private void Start()
    {
        used = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{gameObject.name} was left-clicked!");

            switch (type)
            {
                case Type.Combat:
                    break;
            }
        }
    }
    
    private void OnMouseEnter()
    {
        
    }

    private void OnMouseExit()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (nextNode == null) return;

        foreach (var node in nextNode)
        {
            if (node == null) continue;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, node.transform.position);
            Gizmos.DrawSphere(transform.position, 0.15f);
        }
    }
}
