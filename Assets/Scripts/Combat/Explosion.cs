using Unity.Cinemachine;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer, enemyLayer;
    [SerializeField] private float explosionRadius;
    [SerializeField] private int _playerDamage;
    [SerializeField] private int _enemyDamage;

    private void Explode()
    {
        Debug.Log("Explosion happened");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, playerLayer);

        foreach (Collider collider in hitColliders)
        {
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Damageable damageable;
                if (TryGetDamageable(collider, out damageable))
                {
                    damageable.Damage(_playerDamage);
                }
            }
        }

        Destroy(gameObject);
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
