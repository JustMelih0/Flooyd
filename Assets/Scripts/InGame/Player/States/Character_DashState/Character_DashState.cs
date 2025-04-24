using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newDashState", menuName = "StateMachines/Character/DashState", order = 0)]
public class Character_DashState : CharacterState
{
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private bool canDash = true;
    [SerializeField] private GameObject dashObject;

    public override void Enter()
    {
        machine.canTransitionState = false;
        canDash = false;
        character.characterHealth.canDodge = true;
        AudioManager.Instance.PlaySFX("Dash", 0.9f, 1f);
        Instantiate(dashObject, character.transform.position, Quaternion.Euler(0, character.facingRight == 1 ? 0 : 180 , 0));
        character.rgb2D.AddForce(character.facingRight * dashForce * Vector2.right, ForceMode2D.Impulse);
        character.animator.SetTrigger("dashState");
        endDashTimer = character.StartCoroutine(EndDash());
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        if (endDashTimer != null)
        {
            character.StopCoroutine(endDashTimer);
            endDashTimer = null;
        }
        character.characterHealth.canDodge = false;
    }

    public override void HandlePhysics()
    {

    }
    protected Coroutine endDashTimer;
    private IEnumerator EndDash()
    {
        yield return new WaitForSeconds(0.2f);
        endDashTimer = null;
        character.characterHealth.canDodge = false;
        character.rgb2D.linearVelocityX = 0f;
        machine.canTransitionState = true;
        machine.ChangeState(machine.character_LocomotionState);
        character.StartCoroutine(DashCooldown());
    }

    public IEnumerator DashCooldown()
    {
        character.dashBarFiller.fillAmount = 1;
        character.dashBarBorder.SetActive(true);
        float elapsedTime = dashCooldown;
        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            character.dashBarFiller.fillAmount = elapsedTime / dashCooldown;
            yield return null;
        }
        
        character.dashBarBorder.SetActive(false);
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
