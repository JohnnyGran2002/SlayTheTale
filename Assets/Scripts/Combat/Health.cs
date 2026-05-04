using System.Collections;
using System.Collections.Generic;
using Sonity;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Atributes")]
    [Tooltip("the starting and maximum health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    private float _invincibilityDuration;

    [Header("Refrences")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Damageable _damageable;

    [Header("Events")]
    public GameEvent UpdateHealthUI;

    [Header("Sounds")]
    [SerializeField] private SoundEvent hurtSoundEvent;
    [SerializeField] private SoundEvent deathSoundEvent;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    private void OnEnable()
    {
        _damageable.OnDamaged += DamagebleTakeDamage;
    }

    private void OnDisable()
    {
        _damageable.OnDamaged -= DamagebleTakeDamage;
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_invincibilityDuration > 0)
        {
            _invincibilityDuration -= Time.deltaTime;
        }
    }

    public void TakeDamageEvent(Component sender, object data)
    {
        if (data is int)
        {
            int amount = (int)data;
            _currentHealth -= amount;
            UpdateHealthUI.Raise(this, null);
            Debug.Log("damaged" + amount);
            // if (_currentHealth > 0)
            // {
            //     _animator.Play("Take Damage");
            // }

            //Playing Sounds (could be temporary)
            hurtSoundEvent.Play(transform);
        }

        if (_currentHealth <= 0)
        {
            Death();
        }

    }

    public void DamagebleTakeDamage(Damageable dam, int damage)
    {
        if (_invincibilityDuration > 0)
        {
            return;
        }
        _currentHealth -= damage;
        UpdateHealthUI.Raise(this, null);
        _invincibilityDuration = 0.1f;
        Debug.Log("damaged" + damage);
        // if (_currentHealth > 0)
        // {
        //     _animator.Play("Take Damage");
        // }

        //Playing Sounds (could be temporary)
        hurtSoundEvent.Play(transform);

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

        //Playing Sounds (could be temporary)
        deathSoundEvent.Play(transform);
    }
}
