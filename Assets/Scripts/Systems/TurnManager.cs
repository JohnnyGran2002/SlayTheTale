using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private int _currentTurn;
    public enum turnStatus { playerTurn, enemyTurn }
    public turnStatus currentTurnStatus = turnStatus.playerTurn;
    public GameEvent playerTurnStarted, playerTurnEnded, enemyTurnStarted, enemyTurnEnded;

    // Start is called before the first frame update
    void Start()
    {
        _currentTurn = 0;
        playerTurnStarted.Raise(this, null);
    }

    public void OnEndTurn(InputAction.CallbackContext context)
    {
        if (currentTurnStatus == turnStatus.playerTurn && context.performed)
        {
            playerTurnEnded.Raise(this, null);
            currentTurnStatus = turnStatus.enemyTurn;
        }
    }

    public void StartPlayerTurn(Component sender, object data)
    {
        _currentTurn++;
        playerTurnStarted.Raise(this, null);
        currentTurnStatus = turnStatus.playerTurn;
    }

    public void StartEnemyTurn(Component sender, object data)
    {
        enemyTurnStarted.Raise(this, null);
        currentTurnStatus = turnStatus.enemyTurn;
    }
}
