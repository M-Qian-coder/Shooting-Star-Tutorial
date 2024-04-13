using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class AutoDeactivate : MonoBehaviour
{
    [SerializeField]float lifetime=3f;
    [SerializeField]bool destroyGameobject;
    WaitForSeconds waitLifetime;
    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);    
    }
     void OnEnable()
    {
        StartCoroutine(DeactiveCoroutine()); 
    }
    IEnumerator DeactiveCoroutine()
    {
        yield return waitLifetime;
        if(destroyGameobject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
