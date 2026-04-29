using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityAction<Damageable> OnDamaged = delegate { };

    public void Damage()
    {
        OnDamaged(this);
    }
}

