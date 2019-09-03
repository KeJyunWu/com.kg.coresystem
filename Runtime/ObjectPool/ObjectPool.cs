using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
    ///Need to generate Obj List
    [SerializeField]
    List<Object> m_NeedGenerateObj = new List<Object>();
    int m_count;

    Queue m_ObjectPool_Queue;

    Transform m_parent;
    PoolProduct m_product;

    public ObjectPool(Transform _parent, List<Object> _needGenerateObj, int _count, PoolProduct _product)
    {
        m_parent = _parent;
        m_NeedGenerateObj = _needGenerateObj;
        m_count = _count;
        m_product = _product;
    }

    public GameObject ObjectReuse()
    {
        if (m_ObjectPool_Queue.Count == 0)
            return null;

        GameObject _obj = m_ObjectPool_Queue.Dequeue() as GameObject;
        _obj.GetComponent<IPoolObject>().ReuseBehaviour();
        return _obj;
    }

    public void ObjectRecover(GameObject _Object)
    {
        _Object.GetComponent<IPoolObject>().RecoverBehaviour();
        m_ObjectPool_Queue.Enqueue(_Object);
    }

    public void ObjectPoolInit()
    {
        m_ObjectPool_Queue = new Queue();

        int _count = m_NeedGenerateObj.Count;
        for (var i = 0; i < _count * m_count; i++)
        {
            int _index = i % _count;
            GameObject _obj = ObjFactory.CreatObjByPrefab(m_NeedGenerateObj[_index]) as GameObject;
            _obj.transform.SetParent(m_parent);
            _obj.transform.localPosition = new Vector3(0, 0, 0);
            _obj.GetComponent<IPoolObject>().Init(m_product);
            ObjectRecover(_obj);
        }
    }
}