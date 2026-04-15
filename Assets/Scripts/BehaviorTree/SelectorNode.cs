using UnityEngine;
using System.Collections.Generic;

//SelctorNode is a type of CompositeNode that executes its child nodes in order until one of them returns success.
//If one child node returns success, the SelectorNode returns success. If all child nodes return failure, the SelectorNode returns failure.
//selects one of its children to run based on some condition.
public class SelectorNode : CompositeNode
{
    public override NodeState Tick()
    {
        foreach (Node child in children)
        {
            // If a child succed, return imidiately
            NodeState result = child.Tick();
            if (result == NodeState.Success)
            {
                return NodeState.Success;
            }

            // If a child is still running, return running
            if (result == NodeState.Running)
            {
                return NodeState.Running;
            }
        }

        //only if all children failed, return failure
        return NodeState.Failure;
    }
}
