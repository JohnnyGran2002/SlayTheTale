using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTransform;

    private Node _root;

    private void Start()
    {
        /*SelectorNode rootSelector = new SelectorNode();

        SequenceNode attackSequence = new SequenceNode();

        attackSequence.AddChild(new IsPlayerInRangeNode(transform, playerTransform, 5f));

        attackSequence.AddChild(new TestAttackNode());

        rootSelector.AddChild(attackSequence);

        rootSelector.AddChild(new TestMoveToPlayerNode(transform, playerTransform));

        _root = rootSelector;*/
    }

    private void Update()
    {
        _root.Tick();
    }
}
