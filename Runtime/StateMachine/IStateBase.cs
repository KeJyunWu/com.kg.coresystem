using UnityEngine;
using System.Collections;

public abstract class IStateBase
{
    private string m_stateName;
    public string StateName
    {
        get { return m_stateName; }
        set { m_stateName = value; }
    }

    protected StateControl m_stateControl;
    public IStateBase(StateControl _stateControl)
    {
        m_stateControl = _stateControl;
    }

    public abstract void SetParams<T>(T _params);
    public abstract void StateInit();
    public abstract void StateBegin();
    public abstract void StateEnd();
    public abstract void StateUpdate();
    public abstract void ChangeState();

}