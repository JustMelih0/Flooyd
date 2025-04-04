
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterStateMachine : StateMachine
{

    [HideInInspector] public Character character;


    [Header("State Templates")]
    public Character_LocomotionState character_LocomotionStateTemplate;
    public Character_DashState character_DashStateTemplate;
    public Character_AttackState character_AttackStateTemplate;

    [HideInInspector] public Character_LocomotionState character_LocomotionState;
    [HideInInspector] public Character_DashState character_DashState;
    [HideInInspector] public Character_AttackState character_AttackState;


    protected override void Awake()
    {
        character = GetComponent<Character>();
        InitFromSO(character_LocomotionStateTemplate, out character_LocomotionState);
        InitFromSO(character_DashStateTemplate, out character_DashState);
        InitFromSO(character_AttackStateTemplate, out character_AttackState);
    }
    protected void InitFromSO<T>(CharacterState stateTemplate, out T state) where T : CharacterState
    {
        state = (T)Instantiate(stateTemplate);
        state.Initialize(this, character);
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(character_LocomotionState);
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
