using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerHandler : MonoBehaviour {

    private static TimerHandler m_instance;
    public static TimerHandler intance
    {
        get { return m_instance; }
    }

    List<Timer> m_timers = new List<Timer>();

    public Timer NewTimer(Action<Timer> _callBack,float _duration , Action _OnTimerEachSecondCallBack = null)
    {
        Timer _timer = new Timer();
        _timer.Setup(_callBack, _duration, _OnTimerEachSecondCallBack,TimerFinish);
        _timer.Begin();
        m_timers.Add(_timer);

        return _timer;
    }

    void TimerFinish(Timer _time)
    {
        if (m_timers.Contains(_time))
        {
            int a = m_timers.FindIndex(x => x == _time);
            m_timers.RemoveAt(a);
        }
    }

    void Awake()
    {
        m_instance = this;
    }

    void Update()
    {
        for(var i=0;i< m_timers.Count;i++)
        {
            m_timers[i].Update(Time.deltaTime);
        }
    }
}
