using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ListenToGameEvent", story: "Wait for [GameEvent]", category: "Events", id: "50b1d30c1a71766d96857ce2cdb2bad2")]
public partial class ListenToGameEventModifier : Modifier
{
    [SerializeReference] public BlackboardVariable<GameEvent> GameEvent;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

