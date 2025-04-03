using UnityEngine;

[RequireComponent(typeof(HumonoidMobBase))]
public class GoblinStateMachine : Enemies_StateMachine
{
    HumonoidMobBase humonoidMob;
    [Header("State Templates")]
    public Mob_IdleState mob_IdleStateTemplate;
    public Mob_ChaseState mob_ChaseStateTemplate;

    [HideInInspector]public Mob_IdleState mob_IdleState;
    [HideInInspector]public Mob_ChaseState mob_ChaseState;


    protected override void Awake()
    {
        base.Awake();
        humonoidMob = mob as HumonoidMobBase;

        InitFromSO(mob_IdleStateTemplate, out mob_IdleState);
        InitFromSO(mob_ChaseStateTemplate, out mob_ChaseState);
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
            break;
        }
    }
}
