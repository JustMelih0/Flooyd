using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newDashState", menuName = "StateMachines/Character/DashState", order = 0)]
public class Character_DashState : CharacterState
{
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private bool canDash = true;

    public override void Enter()
    {
        machine.canTransitionState = false;
        canDash = false;
        character.rgb2D.AddForce(character.facingRight * dashForce * Vector2.right, ForceMode2D.Impulse);
        //character.animator.SetTrigger("Dash");
        character.StartCoroutine(EndDash());
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

    private IEnumerator EndDash()
    {
        yield return new WaitForSeconds(0.2f);
        machine.canTransitionState = true;
        machine.ChangeState(machine.character_LocomotionState);
        Debug.Log("End Dash");
        character.StartCoroutine(DashCooldown());
    }

    public IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public override void InputRequest(CharacterInputController.InputType inputType, CharacterInputController.ClickType clickType)
    {
        if (canDash)
        {
            machine.ChangeState(machine.character_DashState);
        }
    }
}
