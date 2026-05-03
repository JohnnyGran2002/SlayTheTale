using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public float _dashSpeed;
    public int _damage;
    private bool _isDashing = false;


    public void StartDashAttack()
    {
        _isDashing = true;
    }

    public void StopDashAttack()
    {
        _isDashing = false;
    }

    public void FixedUpdate()
    {
        if (_isDashing)
        {

            transform.Translate(Vector3.forward * _dashSpeed * Time.deltaTime);
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
}
