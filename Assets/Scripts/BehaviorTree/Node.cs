using System.Runtime.CompilerServices;
using UnityEngine;

//all possilbe states of a node can return
public enum NodeState
{
    Running,
    Success,
    Failure
}


public abstract class Node
{
    //every node must implement this method
    public abstract NodeState Tick();
}
