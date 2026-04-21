using TreeEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTransform;

    public float _attackRange = 2f;

    public float _attackWindup = 1f;

    public float _movementSpeed = 5f;

    private BTNode _root;

    //Build the behavior tree in Start
    private void Start()
    {
        // Build tree manually in code

        SelectorNode rootSelector = new SelectorNode();

        // Attack behavior
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.AddChild(new IsPlayerInRange(transform, playerTransform, _attackRange));
        attackSequence.AddChild(new TestAttackNode(_attackWindup));

        // Movement fallback
        TestMoveToPlayerNode move = new TestMoveToPlayerNode(transform, playerTransform, _movementSpeed);

        // Add to root
        rootSelector.AddChild(attackSequence);
        rootSelector.AddChild(move);

        _root = rootSelector;
    }

    private void Update()
    {
        //tick the behavior tree every frame to determine what the enemy should do
        _root?.Tick();
    }
}
