using UnityEngine;

public class WolfBossStateMachine : GoblinStateMachine
{
    public Mob_DashState mob_DashAttackStateTemplate;

    [HideInInspector] public Mob_DashState mob_DashAttackState;




    protected override void Awake()
    {
        base.Awake();
        InitFromSO(mob_DashAttackStateTemplate, out mob_DashAttackState);
    }
    protected override void Start()
    {
        base.Start();
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
                MobChaseStateControl();
            break;

            case Mob_AttackState:
                MobAttackStateControl();
            break;

            case Mob_DashState:
                MobDashStateControl();
            break;

            case Mob_DeathState:
                stopProcess = true;
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
        else if(humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackCloseRadius)) ChangeState(mob_AttackState);

    }
}
