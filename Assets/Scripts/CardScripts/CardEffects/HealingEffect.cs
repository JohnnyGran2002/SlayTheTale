using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : Effect
{
    [Header("GameEvent")]
    public GameEvent HealPlayer;
    [Header("Effects")]
    [SerializeField] private int _healingAmount;
    public override void PerformEffect()
    {
        HealPlayer.Raise(null, _healingAmount);
    }
}
