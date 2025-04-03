using UnityEngine;

public abstract class Enemies_State : State
{
    protected Enemies_StateMachine machine;
    protected Mob mob;

    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        this.machine = machine as Enemies_StateMachine;
        this.mob = mob;
    }
}
