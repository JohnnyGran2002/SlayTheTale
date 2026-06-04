using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [SerializeField] private float _dashSpeed;
    [SerializeField] private int _damage = 15;

    private bool _isDashing = false, _hasDamagedThisDash = true;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
    }

    public void StartDashAttack()
    {
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _isDashing = true; 
        _hasDamagedThisDash = false;

        //ignore layer 6 (enemies) colliding with itself while dashing, to prevent pushing other enemies
        Physics.IgnoreLayerCollision(6, 6, true);
    }

    public void StopDashAttack()
    {
        _isDashing = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _rigidbody.position = new Vector3(_rigidbody.position.x, transform.position.y, _rigidbody.position.z);

        Physics.IgnoreLayerCollision(6, 6, false);
    }

    public void FixedUpdate()
    {
        if (_isDashing)
        {
            Vector3 velocity = transform.forward * _dashSpeed;
            velocity.y = _rigidbody.linearVelocity.y;

            _rigidbody.linearVelocity = velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDashing) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            StopDashAttack();
            return;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!_isDashing || _hasDamagedThisDash) return;

        if (!_hasDamagedThisDash && collision.gameObject.CompareTag("Player"))
        {
            if (TryGetDamageable(collision.collider, out Damageable dam))
            {
                _hasDamagedThisDash = true;
                dam.Damage(_damage);
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
}
