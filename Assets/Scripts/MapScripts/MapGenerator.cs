using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator mapGenerator;
    
    //To generating the grid of nodes
    [Header("Dependencies"), SerializeField] private MapCameraScript camera;
    [SerializeField] private float cameraHeight;
    [SerializeField, Space(7)] private GameObject nodePrefab;
    [SerializeField] private GameObject bossPrefab;
    
    [Header("Settings"),Space(7), SerializeField, Tooltip("Amount of rows, can't be 0.")] private int rows;
    [SerializeField, Tooltip("Amount of columns, can't be 0.")] private int columns;
    [Tooltip("The space between the nodes.")]public float spaceHorizontal, spaceVertical;
    [SerializeField, Tooltip("AKA number of paths")] private int numberOfStartingRooms;
    [SerializeField, Tooltip("The column number where treasure will be guaranteed. Set to 0 for no treasure floor.")] private int treasureColumn;
    [SerializeField, Tooltip("The column number where no rest or elites can appear under. Set to 0 for no clamp.")] private int eliteAndRestClamp;
    [SerializeField, Space(7), Tooltip("Makes the generation more organic.")] private bool divertingPositions = false;
    [SerializeField] private float verticalDeviationModifier, horizontalDeviationModifier;
    [SerializeField, Space(7)] private NodeLogic.Type[] typeWeight;
    private float _posModifierX, _posModifierY;
    private bool _generated = false;
    
    //To generate paths
    private GameObject[,] _grid;
    private NodeLogic _nodeLogic;
    private GameObject _bossNode;
    private NodeLogic _bossLogic;
    
    //Combat data stuff
    public EnemyData enemyData;
    
    private void Awake()
    {
        if (mapGenerator != null && mapGenerator != this)
        {
            Destroy(this);
        }
        else
        {
            mapGenerator = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        if (_generated) return;
        
        if (rows == 0 || columns == 0)
        {
            Debug.LogWarning("Rows or columns can't be 0");
            return;
        }
        GenerateGrid();
        AssignStartingRooms();
        ConnectEndNodes();
        AssignTypes();
        PruneUnassignedNodes();
        _generated = true;
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
                if (divertingPositions)
                {
                    _posModifierX = Random.Range(-1, 2) * horizontalDeviationModifier;
                    _posModifierY = Random.Range(-1, 2) * verticalDeviationModifier;
                }
                
                GameObject node = Instantiate(nodePrefab, new Vector3(0 + (spaceHorizontal * j) + _posModifierX, 0, 0 + (spaceVertical * i) + _posModifierY), Quaternion.identity, this.transform);
                
                //Changes the name of nodes for easier debugging, can be removed later
                node.name = $"Node_{j}_{i}";

                _grid[j, i] = node;
            }
        }
        
        //Generating boss node
        float middleRow = (rows - 1) / 2f;
        GameObject bossNode = Instantiate(nodePrefab, new Vector3(0 + (spaceHorizontal * middleRow), 0, 0 + (spaceVertical * columns + 1)), Quaternion.identity, this.transform);
        bossNode.name = "Boss_Node";
        _bossLogic = bossNode.GetComponent<NodeLogic>();
        _bossLogic.assigned = true;
        _bossLogic.type = NodeLogic.Type.Boss;
        _bossNode = bossNode;
        
        //Place camera
        camera.startPos = new Vector3(0 + (spaceHorizontal * middleRow), cameraHeight, -10);
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
            AssignNodes(nodePool[i]);
        }
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
    private void AssignTypes()
    {
        //Sets all nodes on column 0 to combats
        for (var i = 0; i < rows; i++)
        {
            GameObject currentNode = _grid[i, 0];
            
            _nodeLogic = currentNode.GetComponent<NodeLogic>();

            if (_nodeLogic.type != NodeLogic.Type.None) continue;

            _nodeLogic.type = NodeLogic.Type.Combat;
        }
        
        //Sets all nodes on treasure column to treasures
        for (var i = 0; i < rows; i++)
        {
            if(treasureColumn == 0) continue;
            
            GameObject currentNode = _grid[i, treasureColumn];
            
            _nodeLogic = currentNode.GetComponent<NodeLogic>();

            if (_nodeLogic.type != NodeLogic.Type.None)continue;

            _nodeLogic.type = NodeLogic.Type.Treasure;
        }
        
        //Sets all nodes on the last column to rests
        for (var i = 0; i < rows; i++)
        {
            GameObject currentNode = _grid[i, columns - 1];
            
            _nodeLogic = currentNode.GetComponent<NodeLogic>();

            if (_nodeLogic.type != NodeLogic.Type.None) continue;

            _nodeLogic.type = NodeLogic.Type.Rest;
        }
        
        //Randomly assigns the rest of the rooms
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                GameObject currentNode = _grid[i, j];
            
                _nodeLogic = currentNode.GetComponent<NodeLogic>();

                if (_nodeLogic.type != NodeLogic.Type.None) continue;

                var randomType = Random.Range(0, typeWeight.Length);

                //Makes sure elites and rest don't appear over a certain friend.
                if (typeWeight[randomType] == NodeLogic.Type.Elite || typeWeight[randomType] == NodeLogic.Type.Rest && j < eliteAndRestClamp && eliteAndRestClamp != 0)
                {
                    _nodeLogic.type = NodeLogic.Type.Shop;
                }
                else
                {
                    _nodeLogic.type = typeWeight[randomType];
                }
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

                _nodeLogic = currentNode.GetComponent<NodeLogic>();

                if (_nodeLogic.assigned) continue;
                
                Destroy(currentNode);
                _grid[i, j] = null;
            }
        }
    }

    public void Move(bool up)
    {
        if (!up)
        {
            foreach (Transform child in transform)
            {
                child.position += Vector3.down * 20.0f;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.position += Vector3.up * 20.0f;
            }
        }
    }

    private void Update()
    {
        //var input = 
    }
}
