using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//can have multiple children, but only one will be active at a time
public abstract class CompositeNode : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    //add a child to the node
    public void AddChild(BTNode node)
    {
        children.Add(node);
    }
}
