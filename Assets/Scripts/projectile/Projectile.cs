using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Projectile : MonoBehaviour
{
    [SerializeField]protected float moveSpeed=10f;
    [SerializeField]protected Vector2 moveDirection;
    protected GameObject target;
    [SerializeField]float damage;
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData[] hitSFX; 
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf) 
        {
            Move();
            yield return null;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            var contactPoint = collision.GetContact(0);
            PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }
    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }
    public  void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
