using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [SerializeField] private float _dashSpeed;
    [SerializeField] private int _damage = 15;
    [SerializeField] private int _dashPerTurn;

    [SerializeField] private Behaviour _behaviour;

    [SerializeField] private GameEvent _enemyTurnEnded;
    [SerializeField] private GameEvent _damagePlayer;

    private bool _isDashing = false;
    private Rigidbody _rigidbody;
    private int _dashCount = 0;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StartDashAttack()
    {
        _isDashing = true;
        _dashCount++;
    }

    public void StopDashAttack()
    {
        _isDashing = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        if (_dashCount >= _dashPerTurn)
        {
            _dashCount = 0;
            _enemyTurnEnded.Raise(this, null);
        }
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
    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDashing) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Damageable dam;
            if (TryGetDamageable(collision.collider, out dam))
            {
                dam.Damage(_damage);
                StopDashAttack();
            }
            Debug.Log("Player damaged");

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
