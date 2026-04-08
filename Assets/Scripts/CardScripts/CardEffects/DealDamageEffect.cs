using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [Header("GameEvent")]
    public GameEvent DamageEnemy;
    [Header("Effects")]
    [SerializeField] private int _damageAmount;

    public override void PerformEffect()
    {
        DamageEnemy.Raise(null, _damageAmount);
    }
}
