using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private int _turnStartHandSize;
    [SerializeField] private int _amountToDraw;
    public GameEvent drawCard, discardCard;
    public void PlayerTurnStartedEvent(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.PlayerTurn) return;
        drawCard.Raise(this, _turnStartHandSize);

        ManaSystem.Instance.RefillManaEvent(this, null);
    }

    public void PlayerTurnEndedEvent(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.PlayerTurn) return;
        discardCard.Raise(this, _amountToDraw);
    }
}
