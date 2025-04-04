using UnityEngine;

[RequireComponent(typeof(MeleeEnemies))]
public class GoblinStateMachine : Enemies_StateMachine
{
    AttackableNPCBase humonoidMob;
    [Header("State Templates")]
    public Mob_IdleState mob_IdleStateTemplate;
    public Mob_ChaseState mob_ChaseStateTemplate;
    public Mob_AttackState mob_AttackStateTemplate;

    [HideInInspector] public Mob_IdleState mob_IdleState;
    [HideInInspector] public Mob_ChaseState mob_ChaseState;
    [HideInInspector] public Mob_AttackState mob_AttackState;


    protected override void Awake()
    {
        base.Awake();
        humonoidMob = mob as AttackableNPCBase;

        InitFromSO(mob_IdleStateTemplate, out mob_IdleState);
        InitFromSO(mob_ChaseStateTemplate, out mob_ChaseState);
        InitFromSO(mob_AttackStateTemplate, out mob_AttackState);
    }
    protected override void Start()
    {
        base.Start();
        ChangeState(mob_IdleState);
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
                if(!humonoidMob.IsEnemyInViewRange(mob_ChaseState.viewRange)) ChangeState(mob_IdleState);
                else if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackRadius)) ChangeState(mob_AttackState);
            break;

            case Mob_AttackState:
                if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackRadius)) mob_AttackState.Enter();
                else ChangeState(mob_IdleState);
            break;
        }
    }
}
