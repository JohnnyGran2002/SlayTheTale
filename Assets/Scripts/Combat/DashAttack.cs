using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public float _dashSpeed;
    public float _dashDistance;
    public int _damage;


    public void PerformDashAttack()
    {
        transform.Translate(Vector3.forward * _dashDistance * _dashSpeed * Time.deltaTime);
    }
}
