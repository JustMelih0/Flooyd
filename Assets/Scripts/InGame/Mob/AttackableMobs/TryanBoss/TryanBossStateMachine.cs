using UnityEngine;

public class TryanBossStateMachine : GoblinStateMachine
{
    public Mob_TeleportAttackState mob_TeleportAttackStateTemplate;
    public Mob_SwordThrowAndCatchState mob_SwordThrowAndCatchStateTemplate;
    public Mob_AttackState mob_AttackState2Template;
    [HideInInspector] public Mob_TeleportAttackState mob_TeleportAttackState;
    [HideInInspector] public Mob_SwordThrowAndCatchState mob_SwordThrowAndCatchState;
    [HideInInspector] public Mob_AttackState mob_AttackState2;

    protected override void Awake()
    {
        base.Awake();
        InitFromSO(mob_TeleportAttackStateTemplate, out mob_TeleportAttackState);
        InitFromSO(mob_SwordThrowAndCatchStateTemplate, out mob_SwordThrowAndCatchState);
        InitFromSO(mob_AttackState2Template, out mob_AttackState2);
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
                BaseChaseControl();
            break;

            case Mob_AttackState:
                BaseAttackControl();
            break;

            case Mob_TeleportAttackState:
                ChangeState(mob_ChaseState);

            break;

            case Mob_SwordThrowAndCatchState:
                if(mob_TeleportAttackState.isEnemyFarEnough)
                ChangeState(mob_TeleportAttackState);

                else ChangeState(mob_ChaseState);
            break;

        }
    }
    protected override void BaseAttackControl()
    {
        int rnd = Random.Range(0,2);
        if (humonoidMob.IsEnemyInAttackRange(mob_AttackState.attackCloseRadius))
        {
            if(rnd == 0) ChangeState(mob_AttackState);
            else ChangeState(mob_AttackState2);
        }
        else ChangeState(mob_ChaseState);
    }
    protected override void BaseChaseControl()
    {
        if(mob_SwordThrowAndCatchState.isEnemyFarEnough)
        {
            ChangeState(mob_SwordThrowAndCatchState);
        }
        else if (mob_TeleportAttackState.isEnemyFarEnough)
        {
            ChangeState(mob_TeleportAttackState);
        }
        base.BaseChaseControl();
    }
    
}
