using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
///<summary>
///
///</summary>
public class Pool
{
    public int Size => size;
    public int RuntimeSize => queue.Count;
    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
    }
    Transform parent;
    [SerializeField]GameObject prefab;
    [SerializeField] int size = 1;
    Queue<GameObject> queue;
    /// <summary>
    /// 根据面板size的大小初始化一个队列
    /// </summary>
    /// <param name="parent">初始化队列的父物体</param>
    public void Initialize(Transform parent)
    {
        this.parent = parent;
        queue=new Queue<GameObject>();
        for(var i=0;i<size;i++)
        {
            queue.Enqueue(Copy());
        }
    }
    /// <summary>
    /// 实例化一个游戏对象，隐藏该游戏对象，这个游戏对象在游戏面板拖入
    /// </summary>
    /// <returns>实例化的游戏对象</returns>
    GameObject Copy()
    {
       var copy= GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);
        return copy;
    }
    /// <summary>
    /// 查找对象池中可用的游戏对象
    /// </summary>
    /// <returns>查找到的游戏对象</returns>
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        if(queue.Count>0&&!queue.Peek().activeSelf)
        {
            availableObject=queue.Dequeue();
        }
        else
        {
            availableObject=Copy();
        }
        queue.Enqueue(availableObject);
        return availableObject;
    }
    /// <summary>
    /// 准备游戏对象，找到队列中可用的游戏对象，并显示它
    /// </summary>
    /// <returns>返回该对象</returns>
    public GameObject prepareObject()
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        return prepareObject;
    }

    /// <summary>
    /// 准备游戏对象，找到队列中可用的游戏对象，并在<param name="position">位置显示它
    /// </summary>
    /// <param name="position">对象出现的位置</param>
    /// <returns>准备的对象</returns>
    public GameObject prepareObject(Vector3 position)
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        return prepareObject;
    }
    /// <summary>
    /// 准备游戏对象，找到队列中可用的游戏对象，并在<param name="position">位置，<param name="rotation">角度显示它
    /// </summary>
    /// </summary>
    /// <param name="position">对象出现的位置</param>
    /// <param name="rotation">对象出现的角度</param>
    /// <returns>准备的对象</returns>
    public GameObject prepareObject(Vector3 position,Quaternion rotation)
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;
        return prepareObject;
    }
    /// <summary>
    /// 准备游戏对象，找到队列中可用的游戏对象，并在<param name="position">位置，<param name="rotation">角度，<param name="localScale">缩放显示它
    /// </summary>
    /// <param name="position">对象出现的位置</param>
    /// <param name="rotation">对象出现的角度</param>
    /// <param name="localScale">对象出现时的缩放</param>
    /// <returns></returns>
    public GameObject prepareObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;
        prepareObject.transform.localScale=localScale;
        return prepareObject;
    }
    /// <summary>
    /// 返回/回收游戏对象，将它入列
    /// </summary>
    /// <param name="gameObject">要回收的游戏对象</param>
    public void Return(GameObject gameObject)
    {
        queue.Enqueue(gameObject);
    }
}
