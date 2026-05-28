using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityAction<Damageable, int> OnDamaged = delegate { };
    public UnityAction<Damageable, Status> OnStatus = delegate { }; 

    public void Damage(int damage)
    {
        Component buffer;
        if (TryGetComponent(typeof(Statmanager), out buffer))
        {
            Statmanager stat = (Statmanager)buffer;
            damage = stat.ModifyRecievedDamage(damage);
        }
        OnDamaged(this, damage);
    }

    public void ApplyStatus(Status status)
    {
        OnStatus(this, status);
    }

    public void DamageAndStatus(int damage, Status status)
    {
        StartCoroutine(DamageThenStatus(damage, status));
    }

    IEnumerator DamageThenStatus(int damage,Status status)
    {
       Damage(damage);
       yield return null;
       ApplyStatus(status);
    }
}

