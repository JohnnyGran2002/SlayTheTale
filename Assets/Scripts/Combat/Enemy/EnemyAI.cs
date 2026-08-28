using System;
using Unity.Behavior;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private bool useAttackQueue = true;
    [SerializeField] private GameEvent _finishedAttack;
    private BehaviorGraphAgent agent;
    private BlackboardReference _blackboard;
    private Vector3 _originalPosition;

    public bool UseAttakQueue
    {
        get { return useAttackQueue; }
    }

    private void OnEnable()
    {
        agent = GetComponent<BehaviorGraphAgent>();
        _blackboard = agent.BlackboardReference;
        _originalPosition = transform.position;
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
        if (transform.position.y != _originalPosition.y)
        {
            transform.position = new Vector3(transform.position.x, _originalPosition.y, transform.position.z);
        }
    }
}
