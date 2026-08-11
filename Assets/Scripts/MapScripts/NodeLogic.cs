using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NodeLogic : MonoBehaviour, IPointerClickHandler
{
    //Generation
    public bool assigned, used;
    public List<GameObject> nextNode = new List<GameObject>();
    public int tier = 69;
    
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

    private void LateUpdate()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{gameObject.name} was left-clicked!");
            switch (type)
            {
                case Type.Combat:
                    MapGenerator.mapGenerator.enemyData = 
                        CombatScenesHolder.combatScenesHolder.tierSettings[tier].enemyData[CombatScenesHolder.combatScenesHolder.combatCounter];
                    SceneManager.LoadScene
                        (
                            CombatScenesHolder.combatScenesHolder.tierSettings[tier].scenesToUse
                            [Random.Range(0, CombatScenesHolder.combatScenesHolder.tierSettings[tier].scenesToUse.Length - 1)]
                        );
                    break;
                case Type.None:
                    break;
                case Type.Elite:
                    break;
                case Type.Event:
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
