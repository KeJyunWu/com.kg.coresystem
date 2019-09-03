using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathMover : MonoBehaviour {

    public Action OnPathBegin;
    public Action OnPathEnd;
    public float m_moveSpeed = 3;
    public float curremtProcess
    {
        get
        {
            iTween _tween = m_mover.GetComponent<iTween>();
            if (_tween == null)
                return 0;
            else
                return _tween.time;
        }
    }

    GameObject m_mover;

    public GameObject PathBegin(PathData _pathdata)
    {
        if(OnPathBegin!=null)
            OnPathBegin.Invoke();
        Vector3[] _path = _pathdata.GetPath();
        float _length = iTween.PathLength(_path);

        m_mover.transform.position = _path[0];

        Hashtable args = new Hashtable();
        args.Add("path", _path);
        args.Add("time", _length * m_moveSpeed);
        args.Add("orienttopath", true);
        args.Add("looktime", 0.6f);
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("oncomplete", "PathEnd");
        args.Add("oncompletetarget",gameObject);
        iTween.MoveTo(m_mover,args);
        return m_mover;
    }

    void PathEnd()
    {
        if (OnPathEnd != null)
            OnPathEnd.Invoke();
    }

    void Start()
    {
        m_mover = new GameObject();
        m_mover.name = "mover";
        m_mover.hideFlags = HideFlags.HideInHierarchy;
    }
}
