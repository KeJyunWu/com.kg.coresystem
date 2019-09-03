using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SpriteAnimationControl : MonoBehaviour
{

    public System.Action OnFinish;

    public bool IsPlay
    {
        get { return m_animator.GetBool("Play"); }
    }

    public float Speed
    {
        get { return m_animator.GetFloat("Speed"); }
    }

    Animator m_animator;

    // Use this for initialization
    public void Play()
    {
        m_animator.SetBool("Play", true);
    }

    public void Stop()
    {
        m_animator.SetBool("Play", false);
    }

    public void SetSpeed(float _speed)
    {
        m_animator.SetFloat("Speed", _speed);
    }

    void FrameEnd()
    {
        Stop();
        OnFinish.Invoke();
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
}
