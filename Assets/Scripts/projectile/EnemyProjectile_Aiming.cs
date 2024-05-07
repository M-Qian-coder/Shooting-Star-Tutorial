using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyProjectile_Aiming : Projectile
{
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoriutine));
        base.OnEnable();
    }
    IEnumerator MoveDirectionCoriutine()
    {
        yield return null;
        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }

    }
}
