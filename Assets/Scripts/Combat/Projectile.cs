using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum enemy { Player, Enemy }
    [SerializeField] int _damage;
    [SerializeField] float _speed;

    private Rigidbody _rigidbody;
    private enemy _enemy;
    private GameObject _target;
    private Vector3 _direction;

    void Start()
    {
        if (_enemy == enemy.Player)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
            _direction = new Vector3(_target.transform.position.x - transform.position.x, transform.position.y, _target.transform.position.z - transform.position.z).normalized;
        }
        else
        {
            _direction = transform.forward;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
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

    private void OnTriggerEnter(Collider other)
    {
        Damageable dam;
        if (TryGetDamageable(other, out dam))
        {
            if (other.tag != "Player")
            {
                dam.Damage(_damage);
            }
        }
        Destroy(gameObject);
    }
}
