using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [SerializeField] private float _dashSpeed;
    [SerializeField] private int _damage = 15;
    [SerializeField] private int _dashPerTurn;

    [SerializeField] private Behaviour _behaviour;
    [SerializeField] private GameEvent EndEnemyTurn;

    private bool _isDashing = false, _hasDamagedThisDash = true;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void StartDashAttack()
    {
        _isDashing = true;
        _hasDamagedThisDash = false;
        //ignore layer 6 (enemies) colliding with itself while dashing, to prevent pushing other enemies
        Physics.IgnoreLayerCollision(6, 6, true);
    }

    public void StopDashAttack()
    {
        _isDashing = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        EndEnemyTurn.Raise(this, null);
        Physics.IgnoreLayerCollision(6, 6, false);
    }

    public void FixedUpdate()
    {
        if (_isDashing)
        {
            _rigidbody.linearVelocity = transform.forward * _dashSpeed;
        }
    }

    private void Update()
    {
        if (_isDashing)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.5f))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    StopDashAttack();
                }
            }
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!_isDashing || _hasDamagedThisDash) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Damageable dam;
            if (TryGetDamageable(collision.collider, out dam))
            {
                dam.Damage(_damage);
                _hasDamagedThisDash = true;
                StopDashAttack();
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

    public void DisableBehavior()
    {
        _behaviour.enabled = false;
    }

    public void EnableBehavior()
    {
        _behaviour.enabled = true;
    }
}
