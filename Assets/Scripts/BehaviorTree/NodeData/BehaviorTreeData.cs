using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "BehaviorTree/Tree")]
public class BehaviorTreeData : ScriptableObject
{
    public NodeData RootNode;

    public List<NodeData> Nodes = new List<NodeData>();
}
