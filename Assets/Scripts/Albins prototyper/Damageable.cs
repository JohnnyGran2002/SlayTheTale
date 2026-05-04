using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityAction<Damageable, int> OnDamaged = delegate { };

    public void Damage(int damage)
    {
        OnDamaged(this, damage);
    }
}

