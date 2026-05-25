using UnityEngine;

public class StunAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Collider attackCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void PerformStunPlayer()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackCollider.bounds.center, attackCollider.bounds.extents, attackCollider.transform.rotation, playerLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            PlayerController playerController = hitCollider.GetComponent<PlayerController>();
            Damageable dam;
            if (TryGetDamageable(hitCollider, out dam))
            {
                if (hitCollider.tag == "Player")
                {
                    dam.Damage(_damage);
                }
            }
            if (playerController != null)
            {
                playerController.StunPlayer(stunDuration);
            }
        }
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
