using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Condition/IsPlayerInRange")]
public class IsPlayerInRangeData : NodeData
{
    [SerializeField] private float _attackRange = 1f;
    public override BTNode CreateNode(BTContext context)
    {
        return new IsPlayerInRange(null, null, _attackRange);
    }
}
