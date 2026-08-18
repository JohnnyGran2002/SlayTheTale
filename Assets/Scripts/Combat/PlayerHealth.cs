using UnityEngine;

public class PlayerHealth : Health
{
    protected override void SetHealth()
    {
        if (PlayerStatic.i.isActiveAndEnabled)
        {
            _currentHealth = PlayerStatic.i.currentHealth;
            _maxHealth = PlayerStatic.i.maxHealth;
        }
        else
        {
            Debug.LogWarning("Recommend PlayerStatic in Scene");
            _currentHealth = _maxHealth;
        }
        _healthBar = GetComponent<HealthBar>();
        _healthBar.SetStartingHealth();
    }

    public override void DamagebleTakeDamage(Damageable dam, int damage)
    {
        base.DamagebleTakeDamage(dam, damage);
        /*if (PlayerStatic.i.isActiveAndEnabled)
        {
            PlayerStatic.i.currentHealth = _currentHealth;
        }*/
    }
}
