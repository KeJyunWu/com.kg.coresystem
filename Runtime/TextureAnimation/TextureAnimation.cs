using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TextureAnimation : MonoBehaviour
{
    public List<Texture> m_textures = new List<Texture>();
    public float m_clipLength = 15;

    [SerializeField]
    public UnityEngine.Events.UnityEvent OnFinish;

    bool m_playing = false;
    float m_timer = 0;

    [SerializeField,ReadOnly]
    Texture m_currentFrame;
    public Texture CurrentFrame { get { return m_currentFrame; } }

    [Button]
    public void Reset()
    {
        m_timer = 0;
    }

    [Button]
    public void Play()
      {
        m_playing = true;
      }

    [Button]
    public void Stop()
      {
        m_playing = false;
        Reset();
      }

    private void Update()
    {
        if (m_playing)
        {
            m_timer += Time.deltaTime;
            if(m_timer>= m_clipLength)
            {
                m_playing = false;
                OnFinish?.Invoke();
            }
        }

        m_currentFrame = m_textures[(int)((m_timer  / m_clipLength) * (m_textures.Count-1))];
    }
}
