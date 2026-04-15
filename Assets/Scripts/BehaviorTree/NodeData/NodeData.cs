using UnityEngine;

public abstract class NodeData : ScriptableObject
{
    public Vector2 Position;

    public abstract Node CreateNode();
}
