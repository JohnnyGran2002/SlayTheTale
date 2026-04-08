using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Atributes")]
    [Tooltip("the starting and maximum health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [Header("Refrences")]
    [SerializeField] private Animator _animator;

    [Header("Events")]
    public GameEvent UpdateHealthUI;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamageEvent(Component sender, object data)
    {
        if (data is int)
        {
            int amount = (int)data;
            _currentHealth -= amount;
            UpdateHealthUI.Raise(this, null);
            if (_currentHealth > 0)
            {
                _animator.Play("Take Damage");
            }
        }

        if (_currentHealth <= 0)
        {
            Death();
        }

    }

    public void HealEvent(Component sender, object data)
    {
        if (data is int)
        {
            int amount = (int)data;
            _currentHealth += amount;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            UpdateHealthUI.Raise(this, null);
        }

    }

    private void Death()
    {
        Debug.Log(gameObject.name + " died");
        _animator.Play("Die");
    }
}
