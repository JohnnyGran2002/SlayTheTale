using System;
using Unity.VisualScripting;
using UnityEngine;

public class Dude : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _damageable.OnDamaged += Die;
    }

    private void Die(Damageable dam)
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        _damageable.OnDamaged -= Die;
    }

    void Update()
    {
        
    }
}
