using System.Runtime.CompilerServices;
using UnityEngine;

//all possilbe states of a node can return
public enum NodeState
{
    Running,
    Success,
    Failure
}


public abstract class BTNode
{
    //every node must implement this method, basically the "update" method of the node, it will return the state of the node after it has been ticked
    public abstract NodeState Tick();
}
