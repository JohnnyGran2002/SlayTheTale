using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardEffect : Effect
{
    [Header("GameEvent")]
    public GameEvent DrawCard;
    [Header("Effects")]
    [SerializeField] private int _drawAmount;
    public override void PerformEffect()
    {
        DrawCard.Raise(null, _drawAmount);
    }
}
