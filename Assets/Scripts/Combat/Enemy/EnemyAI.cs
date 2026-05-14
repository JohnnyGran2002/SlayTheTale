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

    [SerializeField] private BlackboardReference _blackboard;

    public void SetState(EnemyState newSstate)
    {
        _blackboard.SetVariableValue("EnemyState", newSstate);
    }
}
