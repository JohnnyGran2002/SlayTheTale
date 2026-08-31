using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NodeLogic : MonoBehaviour, IPointerClickHandler
{
    //Generation
    public bool assigned, used, inaccessible = true;
    public List<GameObject> nextNode = new List<GameObject>();
    public int tier = 69;
    private NodeLogic _nodeLogic;
    
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

    private string _nextScene;
    private EnemyData _enemyData;

    private void Start()
    {
        used = false;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        Debug.Log($"{gameObject.name} was left-clicked!");

        if (inaccessible)
        {
            Debug.Log("Node is inaccessible.");
            return;
        }
        
        if (used)
        {
            Debug.Log("Node is already used");
            return;
        }
        
        UpdateMapAccessibility();
        
        switch (type)
        {
            case Type.Combat:
                MapGenerator.i.enemyData = 
                    CombatScenesHolder.instance.tierSettings[tier].enemyData[CombatScenesHolder.instance.combatCounter];
                SceneManager.LoadScene
                (
                    CombatScenesHolder.instance.tierSettings[tier].scenesToUse
                        [Random.Range(0, CombatScenesHolder.instance.tierSettings[tier].scenesToUse.Length - 1)]
                );
                MapGenerator.i.Move(false);
                break;
            case Type.None:
                break;
            case Type.Elite:
                break;
            case Type.Event:
                MapEvents.i.LoadEvent(tier);
                break;
            case Type.Shop:
                break;
            case Type.Rest:
                break;
            case Type.Treasure:
                break;
            case Type.Boss:
                break;
            default:
                Debug.Log("how");
                break;
        }
    }

    private void UpdateMapAccessibility()
    {
        foreach (var t in nextNode)
        {
            _nodeLogic = t.GetComponent<NodeLogic>();
            _nodeLogic.inaccessible = false;
        }
        used = true;
        inaccessible = true;
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
