using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AVProQuickTimeManager))]
public class AVProManager : MonoBehaviour {

    private static AVProManager m_instance;
    public static AVProManager intance
    {
        get { return m_instance; }
    }

    GameObject _root;

    List<AVProQuickTimeMovie> m_avproList = new List<AVProQuickTimeMovie>();
    Dictionary<string, AVProQuickTimeMovie> m_avproDictionary = new Dictionary<string, AVProQuickTimeMovie>();

    public AVProQuickTimeMovie GetAVProQuickTimeMovie(string _path)
    {
        AVProQuickTimeMovie _movie;
        if (!m_avproDictionary.ContainsKey(_path))
        {
            _movie = _root.AddComponent<AVProQuickTimeMovie>();
            m_avproDictionary.Add(_path, _movie);
            m_avproList.Add(_movie);
        }
        else
            _movie = m_avproDictionary[_path];

        _movie._playOnStart = false;
        _movie._loadOnStart = false;
        _movie._loop = false;

        return _movie;
    }

    void Awake()
    {
        m_instance = this;
        _root = new GameObject("AVProPool");
        _root.transform.parent = transform;
    }

    void OnDestroy()
    {
        for(var i=0;i< m_avproList.Count;i++)
        {
            m_avproList[i].UnloadMovie();
        }
    }
}
