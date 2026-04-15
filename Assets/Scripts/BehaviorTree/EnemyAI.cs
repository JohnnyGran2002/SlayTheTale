using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField] private float _attackRange = 1f;

    [SerializeField] private float _attackWindup = 1f;

    [SerializeField] private float _movementSpeed = 1f;

    private Node _root;

    private void Start()
    {

        SelectorNode rootSelector = new SelectorNode();

        SequenceNode attackSequence = new SequenceNode();
        attackSequence.AddChild(new IsPlayerInRange(this.transform, playerTransform, _attackRange));
        attackSequence.AddChild(new TestAttackNode(_attackWindup));
        rootSelector.AddChild(attackSequence);

        rootSelector.AddChild(new TestMoveToPlayerNode(this.transform, playerTransform, _movementSpeed));

        _root = rootSelector;
    }

    private void Update()
    {
        _root.Tick();
    }
}
