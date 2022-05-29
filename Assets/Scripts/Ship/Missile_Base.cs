using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Missile_Base : PoolingItemBase
{
    public SO_MissileOnHitEffect OnHitEffect;
    public float TravelSpeed;

    public ShipWeapons ParentShip
    {
        get { return _parentShip; }
        set
        {
            _parentShip = value;
        }
    }

    private ShipWeapons _parentShip;

    private CircleCollider2D col;
    private Rigidbody2D r_body;

    private void Awake()
    {
        //get the collider..
        col = GetComponent<CircleCollider2D>();
        //set it as trigger
        col.isTrigger = true;

        r_body = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //check if we can damage the hit object..
        IDamagable damageable = other.GetComponentInParent<IDamagable>();
        if (damageable != null)
        {
            //.. then damage it
            if (ParentShip != null)
            {
                damageable.TakeDamage(ParentShip);
            }

            //create a missile effect at the position
            OnHitEffect.MissileOnHit(transform);

            //destory the missle
            Destroy(gameObject);
        }
    }

    protected Vector2 Movement()
    {
        Vector2 movementVec = transform.up * TravelSpeed;
        //basic missle with forward movement
        return movementVec;
    }

    protected void Update()
    {
        //update movement
        r_body.velocity = Movement();
    }
}