using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetAcquireVoice = null;
    [Header("------SPEED CHANGE------")]
    [SerializeField]float lowSpeed = 8f;
    [SerializeField]float highSpeed = 24f;
    [SerializeField]float varlableSpeedDelay = 0.5f;
    WaitForSeconds waitvarlableSpeedDelay;
    [Header("---EXPLOSION----")]
    [SerializeField] GameObject explosionVFX = null;
    [SerializeField] AudioData explosionSFX = null;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] LayerMask enemyLayermask = default;
    [SerializeField] float explosionDamage = 100f;
    protected override void Awake()
    {
        base.Awake();
        waitvarlableSpeedDelay=new WaitForSeconds(varlableSpeedDelay);
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PoolManager.Release(explosionVFX, transform.position);
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        var colliders=Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayermask);
        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }
    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;
        yield return waitvarlableSpeedDelay;
        moveSpeed=highSpeed;
        if(target!=null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquireVoice);
        }
    }
}
