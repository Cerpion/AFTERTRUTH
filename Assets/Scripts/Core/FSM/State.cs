using System;

public abstract class State<T> where T : Enum
{
    public Action<T> ChangeState;
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate(float delta) { }
    public virtual void CheckTransition() { }
}
