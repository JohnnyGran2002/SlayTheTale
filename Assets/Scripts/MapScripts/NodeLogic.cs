using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NodeLogic : MonoBehaviour, IPointerClickHandler
{
    
    [Serializable]
    struct StumpMaterials
    {
        public Material baseMaterial;
        public Material glowMaterial;
    }
    
    
    //Generation
    public bool assigned, used, inaccessible = true;
    public List<GameObject> nextNode = new List<GameObject>();
    public int tier = 69;
    public int column;
    private NodeLogic _nodeLogic;
    
    //Stuff
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject lilDude;
    [SerializeField] private MeshRenderer stump;
    
    [SerializeField] private StumpMaterials combatMaterial;
    [SerializeField] private StumpMaterials eventMaterial;
    [SerializeField] private StumpMaterials restMaterial;
    [SerializeField] private StumpMaterials bossMaterial;
    
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
        lilDude.SetActive(false);
        used = false;
        
        foreach (var node in nextNode)
        {
            var currentLine = Instantiate(linePrefab, transform);
            var lineRenderer = currentLine.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, node.transform.position);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        Debug.Log($"{gameObject.name} was left-clicked!");

        if (inaccessible || column < MapGenerator.instance.pointOfNoReturn)
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
                MapGenerator.instance.enemyData = 
                    CombatScenesHolder.instance.tierSettings[tier].enemyData[CombatScenesHolder.instance.combatCounter];
                SceneManager.LoadScene
                (
                    CombatScenesHolder.instance.tierSettings[tier].scenesToUse
                        [Random.Range(0, CombatScenesHolder.instance.tierSettings[tier].scenesToUse.Length - 1)]
                );
                CombatScenesHolder.instance.combatCounter++;
                MapGenerator.instance.Move(false);
                break;
            case Type.None:
                break;
            case Type.Elite:
                break;
            case Type.Event:
                MapEvents.instance.LoadEvent(tier);
                break;
            case Type.Shop:
                break;
            case Type.Rest:
                break;
            case Type.Treasure:
                break;
            case Type.Boss:
                SceneManager.LoadScene(CombatScenesHolder.instance.bossScene);
                break;
            default:
                Debug.Log("how");
                break;
        }
        lilDude.SetActive(false);
    }

    private void UpdateMapAccessibility()
    {
        foreach (var t in nextNode)
        {
            _nodeLogic = t.GetComponent<NodeLogic>();
            _nodeLogic.inaccessible = false;
        }

        MapGenerator.instance.pointOfNoReturn++;
        
        used = true;
        inaccessible = true;
    }
    
    private void OnMouseEnter()
    {
        if (used || inaccessible || column < MapGenerator.instance.pointOfNoReturn) return;
        lilDude.SetActive(true);
    }

    private void OnMouseExit()
    {
        lilDude.SetActive(false);
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

    private void Update()
    {
        if (!used && !inaccessible && column >= MapGenerator.instance.pointOfNoReturn)
        {
            stump.material = type switch
            {
                Type.Combat => combatMaterial.glowMaterial,
                Type.Event => eventMaterial.glowMaterial,
                Type.Rest => restMaterial.glowMaterial,
                _ => bossMaterial.glowMaterial
            };
        }
        else
        {
            stump.material = type switch
            {
                Type.Combat => combatMaterial.baseMaterial,
                Type.Event => eventMaterial.baseMaterial,
                Type.Rest => restMaterial.baseMaterial,
                _ => bossMaterial.baseMaterial
            };
        }
    }
}
