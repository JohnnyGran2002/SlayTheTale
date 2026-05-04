using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;

public class Bomb : MonoBehaviour
{
    public UnityEvent onActivate;
    public float delay;
    public float activeTime;
    public int damage;
    public Collider coll;
    private float timeLeft;
    private bool active;
    void Start()
    {
        timeLeft = delay;
        active = false;
    }

    private void OnValidate()
    {
        Assert.IsTrue(activeTime > 0.0f, "need to be active for more than 0 seconds");
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
                coll.enabled = true;
                onActivate.Invoke();
                timeLeft = activeTime;
            }
            else
            {
                Disable();
                Destroy(gameObject);
            }

        }
    }

    private void Disable()
    {
        coll.enabled = false;
    }
}
