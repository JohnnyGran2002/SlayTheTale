using UnityEngine;

public class TestMoveToPlayerNode : Node
{
    private Transform _enemyTransform;
    private Transform _playerTransform;
    private float _speed = 1f;

    public TestMoveToPlayerNode(Transform enemyTransform, Transform playerTransform, float speed)
    {
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        _speed = speed;
    }

    public override NodeState Tick()
    {
        // Move the enemy towards the player
        _enemyTransform.position = Vector3.MoveTowards(_enemyTransform.position, _playerTransform.position, _speed * Time.deltaTime);

        // Enemy is still moving towards the player
        return NodeState.Running; 
    }
}
