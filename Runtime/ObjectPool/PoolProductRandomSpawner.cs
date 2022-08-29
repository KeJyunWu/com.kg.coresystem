using UnityEngine;
using Sirenix.OdinInspector;

public class PoolProductRandomSpawner : MonoBehaviour
{
    public Vector3 m_center;
    public Vector3 m_size;
    public Vector2 m_randomSizeRange;

    [Title("Automatic Spawn")]
    public int m_count;
    [LabelText("Product")]
    public PoolProduct m_InitProduct = PoolProduct.Product1;

    [Title("Manual Spawn")]
    public KeyCode m_triggerKey = KeyCode.Space;
    [LabelText("Product")]
    public PoolProduct m_TriggerProduct = PoolProduct.Product1;


    void ProductSpawn(PoolProduct _p)
    {
        GameObject _obj = ObjectPoolManager.instance.ObjectReuse(_p);
        _obj.transform.position = CommonFunction.RandomPointInsideBox(m_center, m_size);
        _obj.transform.localScale = Vector3.one * Mathf.Lerp(m_randomSizeRange.x, m_randomSizeRange.y, Random.value);
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
