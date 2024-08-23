using System.Collections;
using System.Collections.Generic;
using System;

public class Timer  {

    Action OnTimerEachSecondCallBack;
    Action<Timer> OnTimerComplete;
    Action<Timer> OnTimerCompleteHandlerCallback;

    float m_duration;
    float m_Timer;
    bool m_bBegin = false;

    float m_EachSecondCBConditionValue = 0;

    public float TotalTime
    {
        get { return m_duration; }
    }

    public float RemainTime
    {
        get { return m_duration - m_Timer; }
    }

    public float CurrentTime
    {
        get { return m_Timer; }
    }


    public void Setup(float _duration, Action<Timer> _OnTimerComplete, Action<Timer> _OnHandlerCallback = null )
    {
        m_duration = _duration;
        OnTimerComplete += _OnTimerComplete;
        OnTimerCompleteHandlerCallback += _OnHandlerCallback;
    }

    public void Begin()
    {
        m_bBegin = true;
    }

    public void Pause()
    {
        m_bBegin = false;
    }

    public void Stop()
    {
        m_bBegin = false;
        Finish();
    }

    public void Release()
    {
        Dispose();
    }

    public void Update(float _deltaTime)
    {
        if(m_bBegin)
        {
            m_Timer += _deltaTime;

            if(m_Timer >= m_EachSecondCBConditionValue)
            {
                m_EachSecondCBConditionValue += 1;
                if (OnTimerEachSecondCallBack != null)
                    OnTimerEachSecondCallBack.Invoke();
            }

            if (m_Timer>m_duration)
            {
                Finish();
                Dispose();
            }
        }
    }

    void Finish()
    {
        if (OnTimerComplete != null && m_bBegin)
        {
            OnTimerComplete.Invoke(this);
        }
        Dispose();
        m_bBegin = false;
    }

    void Dispose()
    {
        OnTimerCompleteHandlerCallback.Invoke(this);
    }
}
