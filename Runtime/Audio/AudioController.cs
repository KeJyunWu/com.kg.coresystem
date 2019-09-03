using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour{

    List<AudioPlayer> m_idlePlayerList = new List<AudioPlayer>();
    List<AudioPlayer> m_usingPlayer = new List<AudioPlayer>();

	public void Init()
    {
        for(var i=0;i<10;i++)
        {
            AudioPlayer _audioPlayer = new AudioPlayer(gameObject);
            m_idlePlayerList.Add(_audioPlayer);
        }
    }

    public void Play(AudioClip _clip, bool _loop, bool _fadeIn)
    {
        AudioPlayer _player = m_idlePlayerList[0];
    }

    public void Stop()
    {

    }

    public void Stop(AudioPlayer _player)
    {

    }

    void AudioPlayerRecover()
    {

    }

}
