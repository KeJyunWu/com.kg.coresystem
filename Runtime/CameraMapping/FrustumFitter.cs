using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UltraCombos.DesignExpo
{
	[ExecuteInEditMode]
	public class FrustumFitter : MonoBehaviour
	{
		public VirtualFrame frame;
		public Camera target;

		Transform dummy = null;

		private void LateUpdate()
		{
			if ( dummy == null )
			{
				var res = GetComponentInChildren<FrustumCamera>();
				if ( res == null )
				{
					var go = new GameObject( "Camera Dummy" );
					go.transform.parent = transform;
					res = go.AddComponent<FrustumCamera>();
				}
				dummy = res.transform;
			}

			if ( target != null )
			{
				float _scale = 1;

				float n = target.nearClipPlane;
				float f = target.farClipPlane;

				var m = frame.transform.localToWorldMatrix;
				Vector3 pa = m.MultiplyPoint( Vector3.Scale( frame.size, new Vector3( -0.5f, -0.5f, 0 ) ) );
				Vector3 pb = m.MultiplyPoint( Vector3.Scale( frame.size, new Vector3( +0.5f, -0.5f, 0 ) ) );
				Vector3 pc = m.MultiplyPoint( Vector3.Scale( frame.size, new Vector3( -0.5f, +0.5f, 0 ) ) );
				Vector3 pe = target.transform.position;

				// Compute an orthonormal basis for the screen.
				Vector3 vr = (pb - pa).normalized;
				Vector3 vu = (pc - pa).normalized;
				Vector3 vn = Vector3.Cross( vu, vr ).normalized;

				// Compute the screen corner vectors.
				Vector3 va = pa - pe;
				Vector3 vb = pb - pe;
				Vector3 vc = pc - pe;

				// Find the distance from the eye to screen plane.
				float d = -Vector3.Dot( va, vn );

				// Find the extent of the perpendicular projection.
				float nd = n / d * _scale;
				float l = Vector3.Dot( vr, va ) * nd;
				float r = Vector3.Dot( vr, vb ) * nd;
				float b = Vector3.Dot( vu, va ) * nd;
				float t = Vector3.Dot( vu, vc ) * nd;

				// Load the perpendicular projection.
				Matrix4x4 P = Matrix4x4.Frustum( l, r, b, t, n, f );

				target.projectionMatrix = P;
				target.transform.rotation = Quaternion.LookRotation( -vn, vu );
			}
		}
	}

}
