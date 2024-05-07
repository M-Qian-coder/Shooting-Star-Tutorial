using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float minBallistcAngle=50f;
    [SerializeField] float maxBallistcAngle=75f;
    float ballistAngle;
    Vector3 targetDirection;
   public IEnumerator HomingCoroutine(GameObject target)
    {
        ballistAngle = Random.Range(minBallistcAngle, maxBallistcAngle);
        while(gameObject.activeSelf)
        {
            if(target.activeSelf)
            {
                 targetDirection = target.transform.position - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f,0f, ballistAngle);
                projectile.Move();
            }
            else
            {
                projectile.Move();
            }
            yield return null;
        }
        
    }
}
