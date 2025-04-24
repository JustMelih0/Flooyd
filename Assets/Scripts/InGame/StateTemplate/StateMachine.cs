using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{

    [Header("Dont Assign Value")]public State currentState;
    public bool canTransitionState = true;
    protected List<State> allStates = new();

    protected virtual void Awake()
    {

    }
    protected virtual void Start() {
        foreach (State item in allStates)
        {
            item.EnabledObject();
        }
    }
    protected virtual void OnEnable()
    {
    }
    protected virtual void OnDisable()
    {
        foreach (State item in allStates)
        {
            item.DisabledObject();
        }
    }
    protected void InitFromBase<T>(T stateTemplate, out T state, Mob mob) where T : State
    {
        state = (T)Instantiate(stateTemplate);
        state.Initialize(this, mob);
        allStates.Add(state);
    }
    private void Update()
    {
        currentState?.Execute();
    }

    private void FixedUpdate()
    {
        currentState?.HandlePhysics();
    }

    public void ChangeState(State newState)
    {
        if (canTransitionState == false) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void AnyState(State newAnyState)
    {
        currentState.Exit();
        currentState = newAnyState;
        currentState.Enter();
    }
    void OnDrawGizmos()
    {
        currentState?.GizmosDraw();
    }
    public void OnAnimatonStarted()
    {
        currentState?.OnAnimationStarted();
    }
    public void OnAnimaton()
    {
        currentState?.OnAnimation();
    }
    public void OnAnimatonEnded()
    {
        currentState?.OnAnimationEnded();
    }

}
