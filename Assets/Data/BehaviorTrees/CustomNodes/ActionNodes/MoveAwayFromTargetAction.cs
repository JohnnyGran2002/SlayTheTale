using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using static Projectile;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move Away From Target", story: "[Self] Navigates Away From [Target]", category: "Action/Navigation", id: "2ab43d2375a5b0dd6b992a7176876c66")]
public partial class MoveAwayFromTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);

    protected override Status OnStart()
    {
        if (Self.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Positions
        Vector3 enemyPos = Self.Value.transform.position;
        Vector3 playerPos = Target.Value.transform.position;

        // Direction AWAY from player
        Vector3 moveDir = (enemyPos - playerPos).normalized;

        // Move enemy
        Self.Value.transform.position += moveDir * Speed.Value * Time.deltaTime;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

