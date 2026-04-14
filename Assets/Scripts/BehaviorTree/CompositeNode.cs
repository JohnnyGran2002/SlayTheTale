using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//can have multiple children, but only one will be active at a time
public abstract class CompositeNode : Node
{
    protected List<Node> children = new List<Node>();

    //add a child to the node
    public void AddChild(Node node)
    {
        children.Add(node);
    }
}
