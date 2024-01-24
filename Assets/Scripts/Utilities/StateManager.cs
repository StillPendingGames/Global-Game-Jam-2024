/*
 * StateManager.cs
 * Scott Duman
 * Sets up a simiple state machine
*/
using UnityEngine;

public class StateManager<T>
{
    protected T m_controller;
    protected IState<T> m_state = null;

    public IState<T> state {
        get { return m_state; }
    }

    public T controller {
        get { return m_controller; }
    }

    public StateManager(T controller) {
        m_controller = controller;
        // m_state = null;
    }

    public virtual void SetState(IState<T> newState) {
        if (newState == null) {
            Debug.LogWarning($"WARNING: Tried to set state to null!");
            return;
        }
        m_state?.OnExit();
        m_state = newState;
        m_state?.OnEnter();
    }

    public bool IsInState(string otherState) {
        if (m_state != null) {
            return m_state.CompareState(otherState);
        }
        return false;
	}

    public bool IsInState(System.Type otherState) {
        if (m_state != null) {
            return m_state.CompareState(otherState);
        }
        return false;
    }

    public void OnUpdate() {
        m_state?.OnUpdate();
    }
}