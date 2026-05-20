using System;
using Unity.Behavior;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameEvent _finishedAttack;

    [SerializeField] private BehaviorGraphAgent agent;
    private BlackboardReference _blackboard;

    private void OnEnable()
    {
        agent = GetComponent<BehaviorGraphAgent>();
        _blackboard = agent.BlackboardReference;
    }

    public void SetState(EnemyStates newState)
    {
         _blackboard.SetVariableValue("EnemyStates", newState);
    }

    private void AttackFinished()
    {
        _finishedAttack.Raise(this, null);
    }
}
