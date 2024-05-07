using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

///<summary>
///
///</summary>
public class PoolManager : MonoBehaviour
{
    static Dictionary<GameObject, Pool> dictionary;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] hitFVX;
    [SerializeField] Pool[] enemypools;
    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialized(playerProjectilePools);
        Initialized(enemyProjectilePools);
        Initialized(hitFVX);
        Initialized(enemypools);
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(hitFVX);
        
    }
    void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if(pool.RuntimeSize>pool.Size)
            {
                Debug.LogWarning((format: "Pool:{0} has a runtime size {1} bigger tahn its initial size {2} "
                    , pool.Prefab.name, pool.RuntimeSize, pool.Size));
            }
        }
    }
#endif
    /// <summary>
    /// 初始化对象池字典，这个字典中有不同对象的池
    /// </summary>
    /// <param name="pools"></param>
    private void Initialized(Pool[] pools)
    {
        foreach (var pool in pools)
        {
        #if UNITY_EDITOR
            if(dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools! Prefab " + pool.Prefab.name);
                continue;
            }
        #endif
            dictionary.Add(pool.Prefab,pool);
            Transform poolParent=new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }
    /// <summary>
    /// 根据传入的预制体参数，从对象池中释放一个对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].prepareObject();
    }
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].prepareObject(position);
    }
    public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].prepareObject(position,rotation);
    }
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].prepareObject(position, rotation,localScale);
    }
}
