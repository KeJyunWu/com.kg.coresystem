using UnityEngine;

public class PoolProductRandomSpawner : MonoBehaviour
{
    public Vector3 m_center;
    public Vector3 m_size;

    [Header("Init Spawn")]
    public int m_count;
    public PoolProduct m_InitProduct = PoolProduct.Product1;

    [Header("Trigger Spawn")]
    public PoolProduct m_TriggerProduct = PoolProduct.Product1;
    public KeyCode m_triggerKey = KeyCode.Space;


    void ProductSpawn(PoolProduct _p)
    {
        GameObject _obj = ObjectPoolManager.instance.ObjectReuse(_p);
        _obj.transform.position = CommonFunction.RandomPointInsideBox(m_center, m_size);
        _obj.transform.parent = this.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(var i=0; i< m_count; i++)
        {
            ProductSpawn(m_InitProduct);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(m_triggerKey))
        {
            ProductSpawn(m_TriggerProduct);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(m_center, m_size);
    }
}
