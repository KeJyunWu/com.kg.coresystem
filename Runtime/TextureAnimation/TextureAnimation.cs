using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureAnimation : MonoBehaviour
{
    public List<Texture> m_textures = new List<Texture>();
    public float m_clipLength = 15;
    public System.Action OnFinish;
    public RawImage m_img;

    [SerializeField,ReadOnly]
    Texture m_currentFrame;

    /*  // Use this for initialization
      public void Play()
      {
          m_animator.SetBool("Play", true);
      }

      public void Stop()
      {
          m_animator.SetBool("Play", false);
      }*/

    private void Update()
    {
        m_img.texture = m_currentFrame = m_textures[(int)(((Time.time % 15) / m_clipLength) * m_textures.Count)];
    }

   /* void FrameEnd()
    {
        Stop();
        OnFinish.Invoke();
    }*/
}
