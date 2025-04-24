using UnityEngine;

[CreateAssetMenu(fileName = "newParryState", menuName = "StateMachines/Character/ParryState", order = 0)]
public class Character_ParryState : CharacterState
{
    [SerializeField]protected GameObject parryEffectInstance;
    public override void Enter()
    {
        machine.canTransitionState = false;
        character.characterHealth.canDodge = true;
        character.rgb2D.linearVelocityX = 0;
        character.animator.SetBool("parried", false);
        character.animator.SetTrigger("parryState");
    }
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
    }
    public override void EnabledObject()
    {
        base.EnabledObject();
        character.characterHealth.BlockedAttackAction += Parried;
    }
    public override void DisabledObject()
    {
        base.DisabledObject();
        character.characterHealth.BlockedAttackAction -= Parried;
    }
    protected void Parried()
    {
        if (machine.currentState == machine.character_ParryState)
        {
            Instantiate(parryEffectInstance, character.attackPoint.position, Quaternion.identity);
            character.animator.SetBool("parried", true);
            machine.canTransitionState = true;
        }
    }
    public override void OnAnimationEnded()
    {
        base.OnAnimationEnded();
        machine.canTransitionState = true;
        machine.ChangeState(machine.character_LocomotionState);
    }
    public override void InputRequest(CharacterInputController.InputType inputType, CharacterInputController.ClickType clickType)
    {
        base.InputRequest(inputType, clickType);
        if (inputType == CharacterInputController.InputType.BlockInput)
        {
            machine.ChangeState(machine.character_ParryState);
        }
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        character.characterHealth.canDodge = false;
    }

    public override void HandlePhysics()
    {
        
    }
}
