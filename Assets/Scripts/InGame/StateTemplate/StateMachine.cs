using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{

    [Header("Dont Assign Value")]public State currentState;
    public bool canTransitionState = true;


    protected virtual void Awake()
    {

    }
    protected virtual void Start() {
        
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
