using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{

    public State currentState;
    public bool canTransitionState = true;


    private void Awake()
    {

    }
    private void InitFromSO<T>(State stateTemplate, out T state) where T : State
    {
        state = (T)Instantiate(stateTemplate);
        state.Initialize(this, null);
    }
    void Start()
    {

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
        currentState?.Gizmos();
    }

}
