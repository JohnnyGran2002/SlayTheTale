using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;


[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Play Animation", story: "[Animator] plays [name] animation", category: "Action/Animation", id: "6e923e65c03de2424b6ff7df5ae3b3f8")]
public partial class PlayAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<string> Name;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Animator.Value.Play(Name.Value);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

