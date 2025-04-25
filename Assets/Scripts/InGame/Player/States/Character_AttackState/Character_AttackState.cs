
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackState", menuName = "StateMachines/Character/AttackState", order = 0)]
public class Character_AttackState : CharacterState
{
    public int damage = 1;
    public int attackIndex = 0;
    public float attackForce = 1f;
    public float maxAttackCombo = 2; 

    public override void Enter()
    {
        machine.canTransitionState = false;
        character.rgb2D.linearVelocityX = 0;

        if (waitComboCoroutine != null)
        {
            character.StopCoroutine(waitComboCoroutine);
        }

        character.animator.SetInteger("attackIndex", attackIndex);
        character.animator.SetTrigger("attackState");
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        if(attackIndex == maxAttackCombo) attackIndex = 0;

        if (waitAndChangeStateCoroutine != null)
        {
            character.StopCoroutine(waitAndChangeStateCoroutine);
            waitAndChangeStateCoroutine = null;
        }
        waitComboCoroutine = character.StartCoroutine(WaitComboCoroutine());
    }

    public override void HandlePhysics()
    {
        
    }

    public override void OnAnimationStarted()
    {
        AudioManager.Instance.PlaySFX("Swoosh", 0.9f, 1f);
        character.rgb2D.AddForce(character.facingRight * attackForce * Vector2.right, ForceMode2D.Impulse);
    }

    public override void OnAnimation()
    {
        Collider2D target = character.IsEnemyInAttackRange();
        if(target == null) return;

        if (target.TryGetComponent(out IHitable hitable))
        {
            CameraController.Instance.Shake();
            hitable.TakeDamage(damage, character.facingRight, false);
        }
    }

    public override void OnAnimationEnded()
    {
        character.rgb2D.linearVelocityX = 0;
        attackIndex++;
        machine.canTransitionState = true;
        waitAndChangeStateCoroutine = character.StartCoroutine(WaitAndChangeState());
    }

    protected Coroutine waitAndChangeStateCoroutine;
    protected IEnumerator WaitAndChangeState()
    {
        yield return new WaitForSeconds(0.05f);
        waitAndChangeStateCoroutine = null;
        machine.ChangeState(machine.character_LocomotionState);
    }

    public override void InputRequest(CharacterInputController.InputType inputType, CharacterInputController.ClickType clickType)
    {
        if (inputType == CharacterInputController.InputType.AttackInput)
        {
            machine.ChangeState(machine.character_AttackState);
        }
    }
    protected Coroutine waitComboCoroutine;
    protected IEnumerator WaitComboCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        waitComboCoroutine = null;
        attackIndex = 0;
    }
}
