/*
 * State.cs
 * Scott Duman
 * Abstract class that sets up a basic state in a state machine
*/

public abstract class IState<T>
{
    protected StateManager<T> m_manager;

    public T controller {
        get { return m_manager.controller; }
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

    public string GetStateName() {
        string name = GetType().ToString();
		return name.Substring(name.LastIndexOf('.') + 1);
    }

    public bool CompareState(string other) {
		return other.Equals(GetStateName());
	}

    public bool CompareState(System.Type other) {
		return GetType() == other;
	}
}