using Mono.Cecil;
using UnityEngine;

//Node that goes through all children, if one fails then failure, if all succeeds then success.
public abstract class SequenceNode : CompositeNode
{
    //tracks the current child node being executed
    private int currentChildIndex = 0;

    public override NodeState Tick()
    {
        while (currentChildIndex < children.Count)
        {
            NodeState childState = children[currentChildIndex].Tick();

            //if a node is still running, pause here
            if (childState == NodeState.Running)
            {
                return NodeState.Running;
            }

            //if a node fails, the whole sequence fails
            if (childState == NodeState.Failure)
            {
                //reset for next time
                currentChildIndex = 0; 
                return NodeState.Failure;
            }

            //move to the next child if all children succeed
            currentChildIndex++;
        }

        //if we get here, all children succeeded
        currentChildIndex = 0; //reset for next time
        return NodeState.Success;
    }
}
