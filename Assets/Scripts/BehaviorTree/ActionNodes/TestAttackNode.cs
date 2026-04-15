using Unity.VisualScripting;
using UnityEngine;

public class TestAttackNode : Node
{
    private float _attackCooldown = 1f;
    private float _currentCooldown = 0f;

    public TestAttackNode(float attackCooldown)
    {
        _attackCooldown = attackCooldown;
    }
    public override NodeState Tick()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
            return NodeState.Running; // Still cooling down
        }

        if (_currentCooldown <= 0)
        {
            Debug.Log("Attacking");
            _currentCooldown = _attackCooldown; // Reset cooldown
        }
        return NodeState.Success;
    }
}
