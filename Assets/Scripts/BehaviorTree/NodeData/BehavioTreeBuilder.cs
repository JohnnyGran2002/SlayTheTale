using UnityEngine;

//converts editor data into runtime nodes
public class BehavioTreeBuilder
{
    public static BTNode BuildTree(NodeData rootNodeData)
    {
        return rootNodeData.CreateNode();
    }
}
