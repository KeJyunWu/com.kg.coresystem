using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjFactory : MonoBehaviour{


    public static Object CreatObjByPrefab(Object _Object)
    {

        if (_Object == null)
            return null;

        Object _obj;
        try
        {
            _obj = Instantiate(_Object);
        }
        catch
        {
            Debug.Log("Create Obj Error");
            _obj = null;
        }
        return _obj;
    }



    public static Object CreateObj(string _SubfolerName = null, string _ObjName = null)
    {
        if (_ObjName == null)
            return null;

        Object _obj;

        try
        {
            _obj = Instantiate(Resources.Load(_SubfolerName + "/" + _ObjName));
        }
        catch
        {
            Debug.Log("Create Obj Error");
            _obj = null;
        }
        return _obj;
    }

    public static void DestroyObj(Object _obj)
    {
        try
        {
            DestroyImmediate(_obj);
        }
        catch
        {
            Debug.Log("Destroy Obj Error");
        }
    }
}
