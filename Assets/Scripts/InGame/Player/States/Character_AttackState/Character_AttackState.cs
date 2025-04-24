
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackState", menuName = "StateMachines/Character/AttackState", order = 0)]
public class Character_AttackState : CharacterState
{
    public int damage = 1;
    public int attackIndex = 0;
    public float attackForce = 1f;
    private bool comboRequested = false; 

    public override void Enter()
    {
        machine.canTransitionState = false;
        character.rgb2D.linearVelocityX = 0;
        attackIndex = 0;
        comboRequested = false;
        character.animator.SetInteger("attackIndex", attackIndex);
        character.animator.SetTrigger("attackState");
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        attackIndex = 0; 
        comboRequested = false;
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

        if (comboRequested && attackIndex == 0)
        {
            character.rgb2D.linearVelocityX = 0;
            attackIndex = 1;
            character.animator.SetInteger("attackIndex", attackIndex);
            comboRequested = false; 
            return;
        }

        attackIndex = 0;
        comboRequested = false;
        machine.canTransitionState = true;
        machine.ChangeState(machine.character_LocomotionState);
    }

    public override void InputRequest(CharacterInputController.InputType inputType, CharacterInputController.ClickType clickType)
    {
        if (inputType == CharacterInputController.InputType.AttackInput)
        {
            if (machine.currentState != machine.character_AttackState)
            {
                attackIndex = 0;
                machine.ChangeState(machine.character_AttackState);
            }
            else
            {
                if (attackIndex == 0) 
                {
                    comboRequested = true;
                }
            }
        }
    }
}
