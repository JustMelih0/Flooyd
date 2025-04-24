using UnityEngine;

[CreateAssetMenu(fileName = "DeathState", menuName = "StateMachines/GeneralStates/DeathState", order = 0)]
public class Mob_DeathState : State
{

    protected StateMachine machine;
    protected Mob mob;
    public override void Enter()
    {
        machine.canTransitionState = false;
        mob.animator.SetTrigger("die");
    }
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        this.machine = machine;
        this.mob = mob;
    }
    public override void OnAnimationEnded()
    {
        Destroy(machine.gameObject);
    }
    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void HandlePhysics()
    {
        
    }
}
