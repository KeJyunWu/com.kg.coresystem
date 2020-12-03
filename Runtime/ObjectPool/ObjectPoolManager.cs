using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolSetting
{
    public PoolProduct m_poolProduct;
    public List<Object> m_product = new List<Object>();
    public int m_count;
}

public enum PoolProduct
{
    Product1,
    Product2,
    Product3,
    Product4,
    Product5,
    Product6,
    Product7,
    Product8,
    Product9,
    Product10
}

public class ObjectPoolManager : MonoBehaviour {

    private static ObjectPoolManager m_instance;
    public static ObjectPoolManager instance
    {
        get { return m_instance; }
    }

    [SerializeField]
    List<PoolSetting> m_poolSetting = new List<PoolSetting>();
    List<ObjectPool> m_Pools = new List<ObjectPool>();
    Dictionary<PoolProduct, ObjectPool> m_poolDictionary = new Dictionary<PoolProduct, ObjectPool>();

    GameObject m_pivot;

    public GameObject ObjectReuse(PoolProduct _poolProduct)
    {
        ObjectPool _p = m_poolDictionary[_poolProduct];
        return _p.ObjectReuse();
    }

    public void ObjectRecover(PoolProduct _poolProduct, GameObject _object)
    {
        ObjectPool _p = m_poolDictionary[_poolProduct];
        _p.ObjectRecover(_object);
    }

    void Awake()
    {
        m_instance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        m_pivot = new GameObject("Pool");
        for (var i=0;i< m_poolSetting.Count;i++)
        {
            PoolSetting _s = m_poolSetting[i];

            GameObject _null = new GameObject(_s.m_poolProduct.ToString());
            _null.transform.position = Vector3.one * 1000;
            _null.transform.parent = m_pivot.transform;

            ObjectPool _p = new ObjectPool(_null.transform, _s.m_product, _s.m_count, m_poolSetting[i].m_poolProduct);
            m_Pools.Add(_p);
            _p.ObjectPoolInit();

            m_poolDictionary.Add(_s.m_poolProduct, _p);
        }
    }
}