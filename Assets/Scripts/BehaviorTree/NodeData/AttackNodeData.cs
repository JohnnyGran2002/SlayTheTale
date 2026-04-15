using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/Attack")]
public class AttackNodeData : NodeData
{
    [SerializeField] private float _attackWindup = 1f;
    public override Node CreateNode()
    {
        return new TestAttackNode(_attackWindup);
    }
}
