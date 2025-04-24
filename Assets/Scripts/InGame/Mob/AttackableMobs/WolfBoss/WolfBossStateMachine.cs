using UnityEngine;

public class WolfBossStateMachine : Enemies_StateMachine
{
    AttackableNPCBase humonoidMob;
    [Header("State Templates")]
    public Mob_IdleState mob_IdleStateTemplate;
    public Mob_ChaseState mob_ChaseStateTemplate;
    public Mob_AttackState mob_AttackStateTemplate;
    public Mob_DashState mob_DashAttackStateTemplate;
    public Mob_DeathState mob_DeathStateTemplate;

    [HideInInspector] public Mob_IdleState mob_IdleState;
    [HideInInspector] public Mob_ChaseState mob_ChaseState;
    [HideInInspector] public Mob_AttackState mob_AttackState;
    [HideInInspector] public Mob_DashState mob_DashAttackState;
    [HideInInspector] public Mob_DeathState mob_DeathState;



    protected override void Awake()
    {
        base.Awake();
        humonoidMob = mob as AttackableNPCBase;

        InitFromSO(mob_IdleStateTemplate, out mob_IdleState);
        InitFromSO(mob_ChaseStateTemplate, out mob_ChaseState);
        InitFromSO(mob_AttackStateTemplate, out mob_AttackState);
        InitFromSO(mob_DashAttackStateTemplate, out mob_DashAttackState);
        InitFromBase(mob_DeathStateTemplate, out mob_DeathState, mob);

    }
    protected override void Start()
    {
        base.Start();
        ChangeState(mob_IdleState);
    }



    public override void MachineProcess()
    {
        if(stopProcess) return;

        if (mob.isDie)
        {
            stopProcess = true;
            AnyState(mob_DeathState);
        }

        switch (currentState)
        {
            case Mob_IdleState: 
                if(humonoidMob.IsEnemyInViewRange(mob_IdleState.viewRange)) ChangeState(mob_ChaseState);
            break;

            case Mob_ChaseState:
                MobChaseStateControl();
            break;

            case Mob_AttackState:
                MobAttackStateControl();
            break;

            case Mob_DashState:
                MobDashStateControl();
            break;
        }
    }
    protected void MobDashStateControl()
    {
        if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackRadius)) ChangeState(mob_AttackState);
        else if(humonoidMob.IsEnemyInViewRange(mob_ChaseState.viewRange)) ChangeState(mob_ChaseState);
        else ChangeState(mob_IdleState);
    }
    protected void MobAttackStateControl()
    {
        int randomValue = Random.Range(0,3);
        if (humonoidMob.IsEnemyInAttackRange(mob_ChaseState.viewRange))
        {
            if (randomValue == 0) ChangeState(mob_DashAttackState);
            else ChangeState(mob_ChaseState);
        }
        else ChangeState(mob_ChaseState);
    }
    protected void MobChaseStateControl()
    {
        if(mob_ChaseState.chaseLimitDone) ChangeState(mob_DashAttackState);
        else if(!humonoidMob.IsEnemyInViewRange(mob_ChaseState.viewRange)) ChangeState(mob_IdleState);
        else if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackRadius)) ChangeState(mob_AttackState);

    }
}
