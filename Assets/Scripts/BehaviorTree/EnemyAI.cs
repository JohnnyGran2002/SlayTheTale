using TreeEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTransform;
    public BehaviorTreeData TreeData;

    private BTNode _root;

    //Build the behavior tree in Start
    private void Start()
    {
        // Create context
        BTContext context = new BTContext
        {
            enemy = transform,
            player = playerTransform
        };

        // Build tree with context
        _root = BehavioTreeBuilder.BuildTree(TreeData.RootNode, context);
    }

    private void Update()
    {
        //tick the behavior tree every frame to determine what the enemy should do
        _root?.Tick();
    }
}
