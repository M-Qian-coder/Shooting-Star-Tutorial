using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]float paddingX;
    [SerializeField]float paddingY;
    [SerializeField]float moveSpeed = 2f;
    [SerializeField]float moveRotationAngle = 25f;
    [Header("Fire")]
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;
    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;
    [SerializeField] AudioData[] projectileLaunchSFX;

    [SerializeField] Vector3 targetPosition;
    private void Awake()
    {
    }
    private void OnEnable()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
        StartCoroutine(nameof(RandomMovingCoroutine));
        StartCoroutine(nameof(RandomFireCoroutine));
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// 随机移动协程，在右边部分屏幕随机移动
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomMovingCoroutine()
    {
        
         targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
        while(gameObject.activeSelf)
        {
            if(Vector3.Distance(transform.position,targetPosition)> moveSpeed*Time.fixedDeltaTime)
            {
                //Debug.Log(Vector3.Distance(transform.position, targetPosition));
                //Debug.Log(Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon);
                   transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime),
                       Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right));
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
               // Debug.Log("重新定位");
            }
            yield return null;
        }
        
    }
    /// <summary>
    /// 随意开火协程
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            yield return  new WaitForSeconds(Random.Range(minFireInterval,maxFireInterval));
            if (GameManager.GameState == GameState.GameOver) yield break; 
           
                foreach (var projectile in projectiles)
                {
                    PoolManager.Release(projectile, muzzle.position);
                }
                AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
           
            }
    }
}
