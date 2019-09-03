using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager m_instance;
    public static AudioManager intance
    {
        get { return m_instance; }
    }

    List<AudioController> m_audioControllerList = new List<AudioController>();

    void Awake()
    {
        m_instance = this;
    }

    public AudioController NewAudioController(string _ControllerName)
    {
        GameObject _object = new GameObject(_ControllerName);
        _object.transform.parent = transform;

        AudioController _audioController = _object.AddComponent<AudioController>();
        _audioController.Init();
        m_audioControllerList.Add(_audioController);

        return _audioController;
    }

    public void DestroyAudioController(AudioController _audioController)
    {
        if(m_audioControllerList.Contains(_audioController))
        {
            int _index = m_audioControllerList.FindIndex(x => x==_audioController);
            GameObject _object = m_audioControllerList[_index].gameObject;
            m_audioControllerList.RemoveAt(_index);
            DestroyImmediate(_object);
        }
    }

}
