using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum enemy { Player, Enemy }

    [SerializeField] int _damage = 10;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _lifetime = 5f;

    private Rigidbody _rigidbody;
    private enemy _enemy = enemy.Player;
    private GameObject _target;
    private Vector3 _direction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0)
        {
            Destroy(gameObject);
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

    private void OnTriggerEnter(Collider other)
    {
        Damageable dam;
        if (TryGetDamageable(other, out dam))
        {
            if (other.tag == "Player")
            {
                dam.Damage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
