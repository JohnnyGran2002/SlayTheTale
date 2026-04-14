using UnityEngine;

public abstract class TestAttackNode : Node
{
    public override NodeState Tick()
    {
        Debug.Log("Attacking");
        return NodeState.Success;
    }
}
