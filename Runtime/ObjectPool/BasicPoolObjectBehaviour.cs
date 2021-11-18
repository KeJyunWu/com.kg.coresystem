using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BasicPoolObjectBehaviour : MonoBehaviour, IPoolObject
{
    [SerializeField, Sirenix.OdinInspector.ReadOnly]
    PoolProduct m_product;
    public UnityEvent m_initEvent;
    public UnityEvent m_recoverEvent;
    public UnityEvent m_reuseEvent;

    public PoolProduct GetPorductType()
    {
        return m_product;
    }

    public void ProductInit(PoolProduct _product)
    {
        m_product = _product;
        m_initEvent?.Invoke();
    }

    public void RecoverBehaviour()
    {
        m_recoverEvent?.Invoke();
    }

    public void ReuseBehaviour()
    {
        m_reuseEvent?.Invoke();
    }
}
