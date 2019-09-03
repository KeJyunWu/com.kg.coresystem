using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayer {

    AudioSource m_audioSource;
    GameObject m_object;

    public AudioPlayer(GameObject _object)
    {
        m_object = _object;
        m_audioSource = m_object.AddComponent<AudioSource>();
    }

    public void Play(AudioClip _clip,bool _loop,bool _fadeIn)
    {
        if(!_loop)
            m_audioSource.PlayOneShot(_clip);
    }


    public void Stop()
    {

    }

    public void Pause()
    {

    }

}
