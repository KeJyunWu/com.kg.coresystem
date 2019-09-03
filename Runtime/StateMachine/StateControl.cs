using UnityEngine;
using System.Collections;

public class StateControl {

	IStateBase m_currentState = null;
    bool m_bFirstRun = false;

    public void setState(IStateBase _StateBase)
    {
        if (m_currentState != null)
            m_currentState.StateEnd();

        m_currentState = _StateBase;
        m_bFirstRun = false;
    }

    public void StateUpdate()
    {
        if (m_currentState == null)
            return;

        if (!m_bFirstRun)
        {
            m_currentState.StateInit();
            m_currentState.StateBegin();
            m_bFirstRun = true;
        }

        m_currentState.StateUpdate();
    }

}
