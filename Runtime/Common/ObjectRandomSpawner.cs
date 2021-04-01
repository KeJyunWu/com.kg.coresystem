using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomSpawner : MonoBehaviour
{
    public Vector3 m_center;
    public Vector3 m_size;
    public int m_count;
    public PoolProduct m_product = PoolProduct.Product1;

    // Start is called before the first frame update
    void Start()
    {
        for(var i=0; i< m_count; i++)
        {
            GameObject _obj = ObjectPoolManager.instance.ObjectReuse(m_product);
            _obj.transform.position = CommonFunction.RandomPointInsideBox(m_center, m_size);
            _obj.transform.parent = this.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(m_center, m_size);
    }
}
