using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newParryState", menuName = "StateMachines/Character/ParryState", order = 0)]
public class Character_ParryState : CharacterState
{
    [SerializeField] protected GameObject parryEffectInstance;
    [SerializeField] protected float coolDownTime = 0.5f;
    [SerializeField] protected float parryFailTime = 2f;
    protected int failedParryCount = 0;
    [SerializeField] protected int maxFailLimit = 2;
    protected Coroutine parryCooldownCoroutine;
    protected Coroutine parryFailResetCoroutine;
    public override void Enter()
    {
        machine.canTransitionState = false;
        character.characterHealth.canDodge = true;
        character.rgb2D.linearVelocityX = 0;
        character.animator.SetBool("parried", false);
        character.animator.SetTrigger("parryState");
        failedParryCount++;
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
            AudioManager.Instance.PlaySFX("Parry", 0.9f, 1f);
            Instantiate(parryEffectInstance, character.attackPoint.position, Quaternion.identity);
            character.animator.SetBool("parried", true);
            machine.canTransitionState = true;
            failedParryCount = 0;
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
        if (inputType == CharacterInputController.InputType.BlockInput && parryCooldownCoroutine == null)
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
        if (parryFailResetCoroutine != null)
        {
            character.StopCoroutine(parryFailResetCoroutine);
            parryFailResetCoroutine = null;
        }

        if (failedParryCount >= maxFailLimit)
        {
            parryCooldownCoroutine = character.StartCoroutine(ParryCooldownCoroutine());
            return;
        }

        parryFailResetCoroutine = character.StartCoroutine(ParryFailCoroutine());

    }
    protected IEnumerator ParryCooldownCoroutine()
    {
        if (parryFailResetCoroutine != null)
        {
            character.StopCoroutine(parryFailResetCoroutine);
            parryFailResetCoroutine = null;
        }
        yield return new WaitForSeconds(coolDownTime);
        failedParryCount = 0;
        parryCooldownCoroutine = null;
    }
    protected IEnumerator ParryFailCoroutine()
    {
        yield return new WaitForSeconds(parryFailTime);
        failedParryCount = 0;
        parryFailResetCoroutine = null;

    }

    public override void HandlePhysics()
    {
        
    }
}
