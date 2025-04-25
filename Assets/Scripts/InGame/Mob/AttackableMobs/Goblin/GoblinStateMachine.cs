using UnityEngine;

[RequireComponent(typeof(MeleeEnemies))]
public class GoblinStateMachine : Enemies_StateMachine
{
    protected AttackableNPCBase humonoidMob;
    [Header("State Templates")]
    public Mob_IdleState mob_IdleStateTemplate;
    public Mob_ChaseState mob_ChaseStateTemplate;
    public Mob_AttackState mob_AttackStateTemplate;
    public Mob_DeathState mob_DeathStateTemplate;

    [HideInInspector] public Mob_IdleState mob_IdleState;
    [HideInInspector] public Mob_ChaseState mob_ChaseState;
    [HideInInspector] public Mob_AttackState mob_AttackState;
    [HideInInspector] public Mob_DeathState mob_DeathState;


    protected override void Awake()
    {
        base.Awake();
        humonoidMob = mob as AttackableNPCBase;

        InitFromSO(mob_IdleStateTemplate, out mob_IdleState);
        InitFromSO(mob_ChaseStateTemplate, out mob_ChaseState);
        InitFromSO(mob_AttackStateTemplate, out mob_AttackState);
        InitFromBase(mob_DeathStateTemplate, out mob_DeathState, humonoidMob);
    }
    protected override void Start()
    {
        base.Start();
        ChangeState(mob_IdleState);
        humonoidMob.mob_Health.mobDeadAction += ()=> AnyState(mob_DeathState);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        humonoidMob.mob_Health.mobDeadAction -= ()=> AnyState(mob_DeathState);
    }



    public override void MachineProcess()
    {
        if(stopProcess) return;

        switch (currentState)
        {
            case Mob_IdleState: 
                if(humonoidMob.IsEnemyInViewRange(mob_IdleState.viewRange)) ChangeState(mob_ChaseState);
            break;

            case Mob_ChaseState:
                BaseChaseControl();
            break;

            case Mob_AttackState:
                BaseAttackControl();
            break;
        }
    }
    protected virtual void BaseAttackControl()
    {
        if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackCloseRadius)) mob_AttackState.Enter();
        else ChangeState(mob_IdleState);
    }
    protected virtual void BaseChaseControl()
    {
        if(!humonoidMob.IsEnemyInViewRange(mob_ChaseState.viewRange)) ChangeState(mob_IdleState);
        else if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackCloseRadius)) ChangeState(mob_AttackState);
    }
}
