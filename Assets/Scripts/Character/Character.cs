using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.Rendering;
using UnityEngine;

///<summary>
///
///</summary>
public class Character : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField]GameObject deathVFX;
    [SerializeField]protected float maxHealth;
    [SerializeField] StatsBar OnHeadHealthbar;
    [SerializeField] bool showOnHeadhealthBar=true;
    [SerializeField]protected float health;
    [SerializeField] AudioData[] deathSFX;
    protected virtual void OnEnable()
    {
        health = maxHealth;
        if(showOnHeadhealthBar)
        {
            ShowOnheadHealthbar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }
    public void ShowOnheadHealthbar()
    {
        OnHeadHealthbar.gameObject.SetActive(true);
        OnHeadHealthbar.Initialize(health,maxHealth);
    }
    public void HideOnHeadHealthBar()
    {
        OnHeadHealthbar.gameObject.SetActive(true);
    }
    public virtual void  TakeDamage(float damage)
    {
        health -= damage;
        if(showOnHeadhealthBar&&gameObject.activeSelf)
        {
            OnHeadHealthbar.UpdateStats(health, maxHealth);
        }
        if(health<=0f)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;
        health=Mathf.Clamp(health+value, 0f, maxHealth);
        if (showOnHeadhealthBar)
        {
            OnHeadHealthbar.UpdateStats(health, maxHealth);
        }
    }
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth*percent);
        }
    }
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health >0f)
        {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }
}
