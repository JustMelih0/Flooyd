
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterStateMachine : StateMachine
{

    [HideInInspector]public Character character;



    public Character_LocomotionState character_LocomotionStateTemplate;

    [HideInInspector]
    public Character_LocomotionState character_LocomotionState;

    private void Awake()
    {
        character = GetComponent<Character>();
        InitFromSO(character_LocomotionStateTemplate, out character_LocomotionState);
    }
    private void InitFromSO<T>(CharacterState stateTemplate, out T state) where T : CharacterState
    {
        state = (T)Instantiate(stateTemplate);
        state.Initialize(this, character);
    }
    void Start()
    {
        currentState = character_LocomotionState;
    }


    private void Update()
    {
        currentState?.Execute();
    }

    private void FixedUpdate()
    {
        currentState?.HandlePhysics();

    }

    public void ChangeState(CharacterState newState)
    {
        if (canTransitionState == false) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void AnyState(CharacterState newAnyState)
    {
        currentState.Exit();
        currentState = newAnyState;
        currentState.Enter();
    }


}
