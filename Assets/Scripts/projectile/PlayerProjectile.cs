using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerProjectile : Projectile
{
   TrailRenderer trail;
    protected virtual void Awake()
    {
        if (moveDirection != Vector2.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
        trail = GetComponentInChildren<TrailRenderer>();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
    }
    private void OnDisable()
    {
        trail.Clear(); 
    }
    
}
