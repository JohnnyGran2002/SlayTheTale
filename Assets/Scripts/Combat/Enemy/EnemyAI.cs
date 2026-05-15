using Unity.Behavior;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameEvent _finishedAttack;

    public enum EnemyState
    {
        Idle,
        Wander,
        Attack
    }

    [SerializeField] private BlackboardReference _blackboard;

    public void SetState(EnemyState newSstate)
    {
        _blackboard.SetVariableValue("EnemyState", newSstate);
    }

    private void AttackFinished()
    {
        _finishedAttack.Raise(this, null);
    }
}
