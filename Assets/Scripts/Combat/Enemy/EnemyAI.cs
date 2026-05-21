using System;
using Unity.Behavior;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameEvent _finishedAttack;

    private BehaviorGraphAgent agent;
    private BlackboardReference _blackboard;
    private float _distanceToPlayer;

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

    private void Update()
    {
        
    }
}
