using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityAction<Damageable, int> OnDamaged = delegate { };
    public UnityAction<Damageable, Status> OnStatus = delegate { }; 

    public void Damage(int damage)
    {
        OnDamaged(this, damage);
    }

    public void ApplyStatus(Status status)
    {
        OnStatus(this, status);
    }
}

