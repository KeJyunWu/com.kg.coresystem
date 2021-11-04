using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BasicPoolObjectBehaviour : MonoBehaviour, IPoolObject
{
    PoolProduct m_product;
    public UnityEvent m_initEvent;
    public UnityEvent m_recoverEvent;
    public UnityEvent m_reuseEvent;

	[Space]
	public bool m_autoRecoverSelf = false;
	[ShowIf( "m_autoRecoverSelf" ), Indent]
	public float m_life = 3f;

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
		if ( m_autoRecoverSelf )
			StartCoroutine( AutoRecover ());
    }

	public IEnumerator AutoRecover()
	{
		yield return new WaitForSeconds( m_life );
		ObjectPoolManager.instance.ObjectRecover( m_product, gameObject);
	}
}
