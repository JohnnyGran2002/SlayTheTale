using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float delay;
    public Collider coll;
    private float timeLeft;
    private bool active;
    void Start()
    {
        timeLeft = delay;
        active = false;
    }

   
    
    public bool TryGetDamageable(Collider other, out Damageable dam)
    {
        dam = other.gameObject.GetComponent<Damageable>();
        if (dam != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            Damageable dam;
            if (TryGetDamageable(other, out dam))
            {
                dam.Damage();
                Destroy(this);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            Damageable dam;
            if (TryGetDamageable(other, out dam))
            {
                dam.Damage();
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            active = true;
        }
    }
    
    
}
