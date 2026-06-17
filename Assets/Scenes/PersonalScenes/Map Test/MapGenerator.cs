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
    
    private List<GameObject> _nodeList = new List<GameObject>();
    private GameObject[,] _grid;
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
        _grid = new GameObject[rows, columns];
        
        //Generates a grid of nodes
        for (var i = 0; i < columns; i++)
        {
            for (var j = 0; j < rows; j++)
            {
                GameObject node = Instantiate(nodePrefab, new Vector3(0 + (spaceHorizontal * j), 0, 0 + (spaceVertical * i)), Quaternion.identity);
                
                //_nodeLogic = node.GetComponent<NodeLogic>();
                node.name = $"Node_{j}_{i}";

                _grid[j, i] = node;
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
        
        nodePool.Sort();
        
        //Assigns nodes
        for (var i = 0; i < numberOfStartingRooms; i++)
        {
            _nodeLogic = _grid[i, 0].GetComponent<NodeLogic>();
            _nodeLogic.assigned = true;
            Debug.Log(nodePool[i] + " is a starting node");
            AssignNodes(nodePool[i]);
        }
    }
    
    private void AssignNodes(int startingRow)
    {
        int currentRow = startingRow;
    
        for (int c = 0; c < columns - 1; c++)
        {
            GameObject previousNode = _grid[currentRow, c];
            
            NodeLogic prevLogic = previousNode.GetComponent<NodeLogic>();

            if (prevLogic.assigned)
            {
                int rowShift = Random.Range(-1, 2);
                int nextRow = currentRow + rowShift;
                    
                if (nextRow < 0) nextRow = 0;
                else if (nextRow >= rows) nextRow = rows - 1;
                        
                GameObject nextNode = _grid[nextRow, c + 1];
                
                NodeLogic nextLogic = nextNode.GetComponent<NodeLogic>();
                
                Debug.Log("I'm " + previousNode.name + " and I want to path to " + nextNode.name);

                if (!prevLogic.nextNode.Contains(nextNode))
                {
                    prevLogic.nextNode.Add(nextNode);
                }
                
                nextLogic.assigned = true;
                        
                currentRow = nextRow;
            }
        }
    }
}
