using System;
using System.Collections.Generic;

public class StateMachine<State> where State : Enum
{
    private State _currentStateId;

    public State<State> _currentState;
    public Dictionary<State, State<State>> _idToState;

    public StateMachine()
    {
        _idToState = new Dictionary<State, State<State>>();
    }

    public void AddState(State stateID, State<State> state)
    {
        _idToState.Add(stateID, state);
        state.ChangeState = ChangeState;
    }

    public void Initialize(State initialState)
    {
        _currentStateId = initialState;
        _currentState = _idToState[initialState];
        _currentState.OnEnter();
    }

    public void Update(float delta)
    {
        _currentState.CheckTransition();
        _currentState.OnUpdate(delta);
    }

    public void ChangeState(State nextState)
    {
        _currentState.OnExit();
        _currentState = GetState(nextState);
        _currentState.OnEnter();
    }

    private State<State> GetState(State bakerStates)
    {
        _currentStateId = bakerStates;
        return _idToState[bakerStates];
    }

    public State CurrentIdState()
    {
        return _currentStateId;
    }
}
