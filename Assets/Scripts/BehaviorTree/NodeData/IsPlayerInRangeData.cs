using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Condition/IsPlayerInRange")]
public class IsPlayerInRangeData : NodeData
{
    [SerializeField] private float _attackRange = 1f;
    public override Node CreateNode()
    {
        return new IsPlayerInRange(null, null, _attackRange);
    }
}
