using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public float _dashSpeed;
    public int _damage;
    private bool _isDashing = false;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StartDashAttack()
    {
        _isDashing = true;
    }

    public void StopDashAttack()
    {
        _isDashing = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
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
            StopDashAttack();
            Debug.Log("Player took damage");
        }


    }
}
