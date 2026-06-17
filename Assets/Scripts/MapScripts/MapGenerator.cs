using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    //To generating the grid of nodes
    [Header("Dependencies"),SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject bossPrefab;
    [Header("Settings"),Space(7), SerializeField] private int rows;
    [SerializeField] private int columns;
    [Tooltip("The space between the nodes")]public float spaceHorizontal, spaceVertical;
    [SerializeField] private int numberOfStartingRooms;
    [SerializeField, Space(7)] private bool divertingPositions = false;
    [SerializeField] private float deviationModifier;
    private float _posModifier = 0;
    
    //To generate paths
    private GameObject[,] _grid;
    private NodeLogic _nodeLogic;
    private GameObject _bossNode;
    private NodeLogic _bossLogic;
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
                //Makes the path slightly more organic if "divertingPositions = true"
                if (divertingPositions) _posModifier = Random.Range(-1, 2) * deviationModifier;
                
                GameObject node = Instantiate(nodePrefab, new Vector3(0 + (spaceHorizontal * j) + _posModifier, 0, 0 + (spaceVertical * i) + _posModifier), Quaternion.identity);
                
                //Changes the name of nodes for easier debugging, can be removed later
                node.name = $"Node_{j}_{i}";

                _grid[j, i] = node;
            }
        }
        //Generating boss node
        float middleRow = (rows - 1) / 2f;
        GameObject bossNode = Instantiate(nodePrefab, new Vector3(0 + (spaceHorizontal * middleRow), 0, 0 + (spaceVertical * columns + 1)), Quaternion.identity);
        bossNode.name = "Boss_Node";
        _bossLogic = bossNode.GetComponent<NodeLogic>();
        _bossLogic.assigned = true;
        _bossNode = bossNode;
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
        
        //Sorts the nodes
        nodePool.Sort();
        
        //Assigns nodes
        for (var i = 0; i < numberOfStartingRooms; i++)
        {
            _nodeLogic = _grid[i, 0].GetComponent<NodeLogic>();
            _nodeLogic.assigned = true;
            Debug.Log(nodePool[i] + " is a starting node");
            AssignNodes(nodePool[i]);
        }
        ConnectEndNodes();
        PruneUnassignedNodes();
    }
    
    private void AssignNodes(int startingRow)
    {
        var currentRow = startingRow;
    
        //Runs a "walker" through every node and checks if it's connected to a previous node or is a starting node
        //If it is, the walker randomly chooses one of three nodes on the column above
        for (var c = 0; c < columns - 1; c++)
        {
            GameObject previousNode = _grid[currentRow, c];
            
            NodeLogic previousLogic = previousNode.GetComponent<NodeLogic>();

            //Makes sure the node is part of a path
            if (!previousLogic.assigned) continue;
            
            //Randomly chooses left (-1), straight (0) or right (1)
            var rowShift = Random.Range(-1, 2);
            var nextRow = currentRow + rowShift;
            
            //Clamps the edges of our grid
            if (nextRow < 0) nextRow = 0;
            else if (nextRow >= rows) nextRow = rows - 1;

            //Prevents crossings of paths
            if (nextRow != currentRow)
            {
                GameObject diagonalNode = _grid[nextRow, c];
                NodeLogic diagonalLogic = diagonalNode.GetComponent<NodeLogic>();

                GameObject targetOfDiagonal = _grid[currentRow, c + 1];

                if (diagonalLogic.nextNode.Contains(targetOfDiagonal))
                {
                    nextRow = currentRow;
                }
            }
            
            //Makes the path connection
            GameObject nextNode = _grid[nextRow, c + 1];
                
            NodeLogic nextLogic = nextNode.GetComponent<NodeLogic>();
                
            Debug.Log("I'm " + previousNode.name + " and I want to path to path to " + nextNode.name);

            if (!previousLogic.nextNode.Contains(nextNode))
            {
                previousLogic.nextNode.Add(nextNode);
            }
                
            nextLogic.assigned = true;
                        
            //Resets the loop
            currentRow = nextRow;
        }
    }

    private void ConnectEndNodes()
    {
        //Connects the last nodes to the boss node
        var endColumnId = columns - 1;
        
        for (var i = 0; i < rows; i++)
        {
            GameObject endNode = _grid[i, endColumnId];
            NodeLogic endLogic = endNode.GetComponent<NodeLogic>();

            if (!endLogic.assigned) continue;

            if (!endLogic.nextNode.Contains(_bossNode))
            {
                endLogic.nextNode.Add(_bossNode);
            }
        }
    }
    
    private void PruneUnassignedNodes()
    {
        //Deletes all unassigned nodes, both from the scene and from the array
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                GameObject currentNode = _grid[i, j];
                
                if (currentNode == null) continue;

                NodeLogic nodeLogic = currentNode.GetComponent<NodeLogic>();

                if (nodeLogic.assigned) continue;
                
                Destroy(currentNode);
                _grid[i, j] = null;
            }
        }
    }
}
