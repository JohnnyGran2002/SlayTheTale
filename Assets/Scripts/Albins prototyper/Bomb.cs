using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float delay;
    private float timeLeft;
    void Start()
    {
        timeLeft = delay;
    }
    
    public Damageable GetDamageable(){
        Vector3 bombPosition = transform.position;
        int layerMask = new LayerMask();
        Collider2D collider = Physics2D.OverlapCircle(bombPosition, radius, layerMask);
        if(collider != null){
            return collider.GetComponent<Damageable>();
        }
        else{
            return null;    
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
           Damageable dam = GetDamageable();
           dam.Damage();
        }
    }
    
    
}
