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
    /// �������size�Ĵ�С��ʼ��һ������
    /// </summary>
    /// <param name="parent">��ʼ�����еĸ�����</param>
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
    /// ʵ����һ����Ϸ�������ظ���Ϸ���������Ϸ��������Ϸ�������
    /// </summary>
    /// <returns>ʵ��������Ϸ����</returns>
    GameObject Copy()
    {
       var copy= GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);
        return copy;
    }
    /// <summary>
    /// ���Ҷ�����п��õ���Ϸ����
    /// </summary>
    /// <returns>���ҵ�����Ϸ����</returns>
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
    /// ׼����Ϸ�����ҵ������п��õ���Ϸ���󣬲���ʾ��
    /// </summary>
    /// <returns>���ظö���</returns>
    public GameObject prepareObject()
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        return prepareObject;
    }

    /// <summary>
    /// ׼����Ϸ�����ҵ������п��õ���Ϸ���󣬲���<param name="position">λ����ʾ��
    /// </summary>
    /// <param name="position">������ֵ�λ��</param>
    /// <returns>׼���Ķ���</returns>
    public GameObject prepareObject(Vector3 position)
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        return prepareObject;
    }
    /// <summary>
    /// ׼����Ϸ�����ҵ������п��õ���Ϸ���󣬲���<param name="position">λ�ã�<param name="rotation">�Ƕ���ʾ��
    /// </summary>
    /// </summary>
    /// <param name="position">������ֵ�λ��</param>
    /// <param name="rotation">������ֵĽǶ�</param>
    /// <returns>׼���Ķ���</returns>
    public GameObject prepareObject(Vector3 position,Quaternion rotation)
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;
        return prepareObject;
    }
    /// <summary>
    /// ׼����Ϸ�����ҵ������п��õ���Ϸ���󣬲���<param name="position">λ�ã�<param name="rotation">�Ƕȣ�<param name="localScale">������ʾ��
    /// </summary>
    /// <param name="position">������ֵ�λ��</param>
    /// <param name="rotation">������ֵĽǶ�</param>
    /// <param name="localScale">�������ʱ������</param>
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
    /// ����/������Ϸ���󣬽�������
    /// </summary>
    /// <param name="gameObject">Ҫ���յ���Ϸ����</param>
    public void Return(GameObject gameObject)
    {
        queue.Enqueue(gameObject);
    }
}
