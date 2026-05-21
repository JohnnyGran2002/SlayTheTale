using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.VFX;

//using Assert = NUnit.Framework.Assert;
[RequireComponent(typeof(VisualEffect))]
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
    public VisualEffectAsset vfx;
    public Attack.AreaType areaType;
    private VisualEffect vs;
    private float timeLeft;
    private bool active;
    
    void Start()
    {
        
    }

    public void playVFX()
    {
        vs.Play();
        Debug.Log("i do do it");
    }
    private void OnEnable()
    {
        timeLeft = delay;
        active = false;
        vs = GetComponent<VisualEffect>();
        vs.visualEffectAsset = vfx;
        vs.Stop();
        transform.SetParent(null,true);
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
                if (CompareTag(TagHandle.GetExistingTag("Player")))
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
                TryDoDamage();
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
                Bounds bounds = new Bounds(transform.position + (transform.forward * (length * 0.5f)), new Vector3(width, 10, length));
                buffer = Physics.OverlapBox(transform.position + (transform.forward * (length * 0.5f)),bounds.extents,transform.rotation);
                foreach (var target in buffer)
                {
                    
                    if (TryGetDamageable(target,out dam) && !target.CompareTag(TagHandle.GetExistingTag("Player")))
                    {
                        dam.Damage(damage);
                        //Debug.Log(damage);
                    }
                }
                break;
            case Attack.AreaType.Cone:
                float start = angle * -0.5f;
                int rayAmount = (int)angle / 2 + (int)length;
                float angleIncrement = angle / rayAmount;
                Ray ray = new Ray();
                RaycastHit hit = new RaycastHit();
                for (int i = 0; i < rayAmount; i++)
                {
                    ray.origin = transform.position;
                    ray.direction = Quaternion.AngleAxis(start + angleIncrement * i, transform.up) * transform.forward;
                    if (Physics.Raycast(ray, out hit, length))
                    {
                        if (TryGetDamageable(hit.collider, out dam) && !hit.collider.CompareTag(TagHandle.GetExistingTag("Player")))
                        {
                            dam.Damage(damage);
                            //Debug.Log(damage);
                        }
                    }
                    
                }
                
                break;
            case Attack.AreaType.Circle:
                buffer = Physics.OverlapCapsule(transform.position + Vector3.down * 10, transform.position + Vector3.up * 10, radius); 
                foreach (var target in buffer)
                {
                    if (TryGetDamageable(target,out dam) && !target.CompareTag(TagHandle.GetExistingTag("Player")))
                    {
                        dam.Damage(damage);
                        //Debug.Log(damage);
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
        switch (areaType)
        {
            case Attack.AreaType.Square:
                Bounds bounds = new Bounds(transform.position + (transform.forward * (length * 0.5f)), new Vector3(width, 10, length));
                Gizmos.DrawWireCube(transform.position + (transform.forward * (length * 0.5f)),bounds.size);
                
                break;
            case Attack.AreaType.Cone:
                float start = angle * -0.5f;
                int rayAmount = (int)angle / 2 + (int)length;
                float angleIncrement = angle / rayAmount;
                for (int i = 0; i < rayAmount; i++)
                {
                    Gizmos.DrawLine(transform.position,transform.position + Quaternion.AngleAxis(start + angleIncrement * i, transform.up) * transform.forward * length);
            
                    
                }
                break;
            case Attack.AreaType.Circle:
                
                Gizmos.DrawWireSphere(transform.position + Vector3.down * 10,radius);
                Gizmos.DrawWireSphere(transform.position + Vector3.up * 10,radius);
                Gizmos.DrawWireSphere(transform.position,radius);
                break;
            default:
                Debug.LogException(new Exception("how did you even?"), this);
                break;
        }
    }
}
