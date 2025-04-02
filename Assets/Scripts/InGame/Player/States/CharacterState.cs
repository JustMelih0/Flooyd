using UnityEngine;

public abstract class CharacterState : State
{
    protected CharacterStateMachine machine;
    protected Character character;
    
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        this.machine = machine as CharacterStateMachine;
        character = mob as Character;
    }

}
