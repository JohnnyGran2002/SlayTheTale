using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    public enum CurrentTurn { playerTurn, enemyTurn}
    // enum turnstatus - start gör x | end gör y 
    public enum TurnStatus {start, active, end}
    
    public CurrentTurn currentTurn;
    public TurnStatus turnStatus;

    [SerializeField] private float _playerTurnTime, _enemyTurnTime;
    //public GameEvent .....

    // Turnmanager -> ecent playerTurn Player 
}
