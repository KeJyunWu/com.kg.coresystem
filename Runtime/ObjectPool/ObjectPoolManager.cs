using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable]
public class PoolSetting
{
    [HorizontalGroup("Group 1"), LabelWidth(150), HideLabel]
    public PoolProduct m_poolProduct;
    [HorizontalGroup("Group 1"), LabelWidth(150), HideLabel]
    public string m_description = "Default";
    [HorizontalGroup("Group 2"), LabelWidth(150), HideLabel]
    public int m_count;
    [HorizontalGroup("Group 2"), LabelWidth(150), HideLabel]
    public List<Object> m_product = new List<Object>();
}

[EnumPaging]
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
    Product10,
    Product11,
    Product12,
    Product13,
    Product14,
    Product15,
    Product16,
    Product17,
    Product18,
    Product19,
    Product20,
}

public class ObjectPoolManager : MonoBehaviour {

    private static ObjectPoolManager m_instance;
    public static ObjectPoolManager instance
    {
        get { return m_instance; }
    }

    [SerializeField]
    GameObject m_rootParent;

    [SerializeField]
    List<PoolSetting> m_poolSetting = new List<PoolSetting>();
    
    List<ObjectPool> m_Pools = new List<ObjectPool>();
    Dictionary<PoolProduct, ObjectPool> m_poolDictionary = new Dictionary<PoolProduct, ObjectPool>();

    GameObject m_pivot;

    public ObjectPool GetPool(PoolProduct _poolProduct)
    {
        ObjectPool _p = m_poolDictionary[_poolProduct];
        return _p;
    }

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

    public string GetPorductDescription(PoolProduct _p)
    {
        var _v = from _source in m_poolSetting
                 where _source.m_poolProduct == _p
                 select _source;

        return _v == null || _v.ToList().Count == 0 ? "Null" : _v.ToList()[0].m_description;
    }

    void Awake()
    {
        m_instance = this;
		Init();
	}

    void Init()
    {
        m_pivot = new GameObject("Pool");
        if (m_rootParent != null)
            m_pivot.transform.parent = m_rootParent.transform;

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
