using UnityEngine;
using System.Collections.Generic;

//converts editor data into runtime nodes
public class BehavioTreeBuilder
{
    public static BTNode BuildTree(NodeData rootNodeData, BTContext context)
    {
        return BuildNode(rootNodeData, context);
    }

    private static BTNode BuildNode(NodeData nodeData, BTContext context)
    {
        //create runtime node
        BTNode node = nodeData.CreateNode(context);

        //if it is a composite, add cildren
        if (node is CompositeNode compositeNode)
        {
            foreach (var childData in nodeData.children)
            {
                compositeNode.AddChild(BuildNode(childData, context));
            }
        }
        return node;
    }
}
