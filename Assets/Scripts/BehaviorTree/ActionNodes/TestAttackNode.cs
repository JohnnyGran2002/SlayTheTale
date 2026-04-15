using Unity.VisualScripting;
using UnityEngine;

public class TestAttackNode : Node
{
    private float _attackWindup = 1f;
    private float _attackCountdown = 0f;

    public TestAttackNode(float attackCooldown)
    {
        _attackWindup = attackCooldown;
        _attackCountdown = _attackWindup;
    }
    public override NodeState Tick()
    {
        if (_attackCountdown > 0)
        {
            _attackCountdown -= Time.deltaTime;
            return NodeState.Running;
        }

        if (_attackCountdown <= 0)
        {
            Debug.Log("Attacking");
            _attackCountdown = _attackWindup;
        }
        return NodeState.Success;
    }
}
