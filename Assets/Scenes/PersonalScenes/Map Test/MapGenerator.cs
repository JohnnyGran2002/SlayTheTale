using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public int rows,columns;
    [Tooltip("The space between the nodes")]public float spaceHorizontal, spaceVertical;
    [Space(7),SerializeField] private int numberOfStartingRooms;
    
    private List<GameObject> _nodesList = new List<GameObject>();
    private NodeLogic _nodeLogic;
    
    void Start()
    {
        if (rows == 0 || columns == 0)
        {
            Debug.LogWarning("Rows or colums can't be 0");
            return;
        }

        GenerateGrid();
        AssignStartingRooms();
    }

    private void GenerateGrid()
    {
        //Generates a grid of nodes
        for (var i = 0; i < columns; i++)
        {
            for (var j = 0; j < rows; j++)
            {
                GameObject node = Instantiate(nodePrefab, new Vector3(0 + (spaceHorizontal * j), 0, 0 + (spaceVertical * i)), Quaternion.identity);
                _nodesList.Add(node);
            }
        }
    }

    private void AssignStartingRooms()
    {
        //Makes a randomized list of possible nodes to choose from. Fun fact: This is apparently called a Fisher-Yates shuffle!
        List<int> nodePool = new List<int>();
        for (var i = 0; i < rows; i++)
        {
            nodePool.Add(i);
        }
        for (var i = nodePool.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (nodePool[i], nodePool[randomIndex]) = (nodePool[randomIndex], nodePool[i]);
        }
        
        //Assigns nodes
        for (var i = 0; i < numberOfStartingRooms; i++)
        {
            AssignNode(nodePool[i]);
            Debug.Log("Generated " + nodePool[i]);
        }
    }
    
    private void AssignNode(int targetNode)
    {
        //Sets the bool "assigned" on target node to true
        _nodeLogic = _nodesList[targetNode].GetComponent<NodeLogic>();
        _nodeLogic.assigned = true;
    }
}
