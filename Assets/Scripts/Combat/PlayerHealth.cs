using UnityEngine;

public class PlayerHealth : Health
{
    protected override void SetHealth()
    {
        if (PlayerStatic.i.isActiveAndEnabled)
        {
            _currentHealth = PlayerStatic.i.currentHealth;
        }
        else
        {
            Debug.LogWarning("Recommend PlayerStatic in Scene");
            _currentHealth = _maxHealth;
        }
    }

    public override void DamagebleTakeDamage(Damageable dam, int damage)
    {
        base.DamagebleTakeDamage(dam, damage);
        if (PlayerStatic.i.isActiveAndEnabled)
        {
            PlayerStatic.i.currentHealth = _currentHealth;
        }
    }
}
