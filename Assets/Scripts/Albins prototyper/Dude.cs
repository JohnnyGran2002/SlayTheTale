using System;
using Unity.VisualScripting;
using UnityEngine;

public class Dude : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;

    [SerializeField] private int Health;

    private int healthCurrent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthCurrent = Health;
    }

    private void OnEnable()
    {
        _damageable.OnDamaged += Die;
    }

    private void Die(Damageable dam, int damage)
    {
        Debug.Log(damage);
        healthCurrent -= damage;
        if (healthCurrent <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(healthCurrent);
    }

    private void OnDisable()
    {
        _damageable.OnDamaged -= Die;
    }

    void Update()
    {
        
    }
}
