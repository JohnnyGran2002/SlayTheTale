using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public GameEvent DamagePlayer;
    public GameEvent PlayerTurnStarted;

    [SerializeField] PlayerDefenseMoves _playerDefenseMoves;
    public void AttackPlayerEvent(Component sender, object data)
    {
        _animator.Play("Attack");
        StartCoroutine(StartPlayerTurn());
    }

    private void TryAttacking(int _damageAmount)
    {
        if (PlayerDefenseMoves.Instance.IsDodgning == true)
        {
            Debug.Log("Player Dodged");
        }
        else
        {
            DamagePlayer.Raise(this, _damageAmount);
            Debug.Log("player took " + _damageAmount + " damage");
        }
    }

    private IEnumerator StartPlayerTurn()
    {
        yield return new WaitForSeconds(3);

        PlayerTurnStarted.Raise(this, null);
    }
}
