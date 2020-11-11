using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UltraCombos.DesignExpo
{
	public class VirtualFrame : MonoBehaviour
	{
		public Vector3 size;

		private void Start()
		{

		}

		private void Update()
		{

		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			using ( new Handles.DrawingScope( Color.cyan, transform.localToWorldMatrix ) )
			{
				Handles.DrawWireCube( Vector3.zero, size );
			}
#endif
		}
	}

}


