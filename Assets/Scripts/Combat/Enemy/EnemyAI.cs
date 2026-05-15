using Unity.Behavior;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Wander,
        Attack
    }

    [SerializeField] private GameEvent _finishedAttack;

    [SerializeField] private BlackboardReference _blackboard;

    public void SetState(EnemyState newSstate)
    {
        // Set the new state in the blackboard
        _blackboard.SetVariableValue("EnemyState", newSstate);
    }

    private void AttackFinished()
    {
        _finishedAttack.Raise(this, null);
    }
}
