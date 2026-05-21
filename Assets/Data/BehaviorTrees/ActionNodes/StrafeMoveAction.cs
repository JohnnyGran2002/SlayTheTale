using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using static Projectile;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Strafe Move ", story: "Move [self] around [target] to the [Left] with [Speed]", category: "Action/Navigation", id: "a9301bd3a829b21ec49e65ebda6b78d1")]
public partial class StrafeMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<bool> Left;
    [SerializeReference] public BlackboardVariable<float> Speed;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value == null || Target.Value == null)
            return Status.Failure;

        Vector3 selfPosition = Self.Value.transform.position;
        Vector3 targetPosition = Target.Value.transform.position;

        // Direction toward player
        Vector3 toPlayer = (targetPosition - selfPosition).normalized;

        // Create strafe direction
        Vector3 strafeDir;

        if (Left.Value)
        {
            strafeDir = Vector3.Cross(Vector3.up, toPlayer);
        }
        else
        {
            strafeDir = Vector3.Cross(toPlayer, Vector3.up);
        }

        // Blend movement
        Vector3 finalDir =
            (strafeDir * 0.8f) + (toPlayer * 0.2f);

        finalDir.Normalize();

        // Move enemy
        Self.Value.transform.position += finalDir * Speed.Value * Time.deltaTime;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

