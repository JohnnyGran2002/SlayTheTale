using UnityEngine;

public class IsPlayerInRange : BTNode
{
    private Transform _enemyTransform;
    private Transform _playerTransform;
    private float _range;

    public IsPlayerInRange(Transform enemyTransform, Transform playerTransform, float range)
    {
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        _range = range;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(_enemyTransform.position, _playerTransform.position);
        //returns success if the player is within the specified range
        if (distance <= _range)
        {
            return NodeState.Success;
        }
        //otherwise returns failure
        return NodeState.Failure;
    }
}

