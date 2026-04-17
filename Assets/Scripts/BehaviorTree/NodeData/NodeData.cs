using System.Collections.Generic;
using UnityEngine;

//basic class for all node data
public abstract class NodeData : ScriptableObject
{
    public Vector2 Position;

    public List<NodeData> children = new List<NodeData>();

    public abstract BTNode CreateNode();
}
