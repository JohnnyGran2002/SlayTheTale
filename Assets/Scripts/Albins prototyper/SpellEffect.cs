using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
//using Assert = NUnit.Framework.Assert;

public class SpellEffect : MonoBehaviour
{
    public UnityEvent onActivate;
    public UnityEvent Onspawn;
    public float delay;
    public float LingerTime;
    public int damage;
    public float length;
    public float width;
    public float radius;
    public float angle;
    public Attack.AreaType areaType;
    private float timeLeft;
    private bool active;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        timeLeft = delay;
        active = false;
        Onspawn.Invoke();
    }

    private void OnValidate()
    {
        //Assert.IsTrue(activeTime > 0.0f, "need to be active for more than 0 seconds");
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
        if (active)
        {
            Damageable dam;
            if (TryGetDamageable(other, out dam))
            {
                if (other.tag != "Player")
                {
                    dam.Damage(damage);
                }
            }
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            if (!active)
            {
                active = true;
                onActivate.Invoke();
                //TryDoDamage();
                timeLeft = LingerTime;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    private void TryDoDamage()
    {
        Collider[] buffer;
        Damageable dam;
        switch (areaType)
        {
            case Attack.AreaType.Square:
                Bounds bounds = new Bounds(transform.position, new Vector3(width, 10, length));
                buffer = Physics.OverlapBox((transform.position + Vector3.forward * length) * 0.5f,bounds.extents * 0.5f,transform.rotation);
                foreach (var target in buffer)
                {
                    if (TryGetDamageable(target,out dam))
                    {
                        dam.Damage(damage);
                    }
                }
                break;
            case Attack.AreaType.Cone:
                
                break;
            case Attack.AreaType.Circle:
                buffer = Physics.OverlapCapsule(transform.position + Vector3.down * 10, transform.position + Vector3.up * 10, radius); 
                foreach (var target in buffer)
                {
                    if (TryGetDamageable(target,out dam))
                    {
                        dam.Damage(damage);
                    }
                }
                break;
            default:
                Debug.LogException(new Exception("how did you even?"), this);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Bounds bounds = new Bounds(transform.position, new Vector3(width, 10, length));
        Gizmos.DrawWireCube((transform.position + Vector3.forward * length) * 0.5f,bounds.size);
        
    }
}
