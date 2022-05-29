using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PowerupInteractor : MonoBehaviour
{
    public float PickupRange = 2f;
    private CircleCollider2D col;
    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = PickupRange;
        col.isTrigger = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, PickupRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerUp powerup = other.GetComponent<PowerUp>();
        if(powerup)
        {
            powerup.UsePowerUp(transform.root.gameObject);
        }
    }
}
