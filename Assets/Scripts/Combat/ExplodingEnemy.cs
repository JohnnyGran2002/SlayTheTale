using UnityEngine;

[RequireComponent (typeof(EnemyAI))]
public class ExplodingEnemy : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void SetToActive(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.EnemyTurn)
        {
            enemyAI.SetState(EnemyStates.Idle);
        }
        else
        {
            enemyAI.SetState(EnemyStates.Wander);
        }
    }

    public void SetToIdle(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.EnemyTurn)
        {
            enemyAI.SetState(EnemyStates.Idle);
        }
        else
        {
            enemyAI.SetState(EnemyStates.Wander);
        }
    }

}
