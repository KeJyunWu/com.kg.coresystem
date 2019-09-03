using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommomFunction : MonoBehaviour {

	public static Vector4 QuaternionToVector4(Quaternion _rotation)
    {
        return new Vector4(_rotation.x, _rotation.y, _rotation.z, _rotation.w);
    }
}
