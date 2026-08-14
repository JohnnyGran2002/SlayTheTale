using Unity.Cinemachine;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int _playerDamage;
    [SerializeField] private int _enemyDamage;
    [SerializeField] private float _lifeTime;
    private bool _hasDamaged;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _hasDamaged == false)
        {
            if (TryGetDamageable(other, out Damageable dam))
            {
                dam.Damage(_playerDamage);
                Debug.Log("Explosion dealt damage to" + other.name);
            }
        }

        if (other.gameObject.tag == "Enemy" && _hasDamaged == false)
        {
            if (TryGetDamageable(other, out Damageable dam))
            {
                dam.Damage(_enemyDamage);
                Debug.Log("Explosion dealt damage to" + other.name);
            }
        }
        _hasDamaged = true;
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
}
