using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField] private float _attackRange = 1f;

    [SerializeField] private float _attackWindup = 1f;

    [SerializeField] private float _movementSpeed = 1f;

    private Node _root;

    //Build the behavior tree in Start
    private void Start()
    {
        //decides behavior priority
        SelectorNode rootSelector = new SelectorNode();

        //attack sequence(condition + action)
        SequenceNode attackSequence = new SequenceNode();
        //check if player is in range
        attackSequence.AddChild(new IsPlayerInRange(this.transform, playerTransform, _attackRange));
        //if true, attack player
        attackSequence.AddChild(new TestAttackNode(_attackWindup));
        //add attack sequence to root selector
        rootSelector.AddChild(attackSequence);

        //fallback behavior if player is not in range, move towards player
        rootSelector.AddChild(new TestMoveToPlayerNode(this.transform, playerTransform, _movementSpeed));

        _root = rootSelector;
    }

    private void Update()
    {
        //tick the behavior tree every frame to determine what the enemy should do
        _root.Tick();
    }
}
