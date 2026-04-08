using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainManaEffect : Effect
{
    [Header("GameEvent")]
    public GameEvent UpdateMana;
    [Header("Effects")]
    [SerializeField] private int _manaAmount;
    public override void PerformEffect()
    {
        UpdateMana.Raise(null, _manaAmount);
    }
}
