using UnityEngine;



[CreateAssetMenu(fileName = "newLocomotionState", menuName = "StateMachines/Character/LocomotionState", order = 0)]
public class Character_LocomotionState : CharacterState
{
    [SerializeField]private float moveSpeed = 1f;
    private float horizontalInput;
    public override void Enter()
    {
        character.animator.SetTrigger("locomotionState");
    }

    public override void Execute()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        character.FaceToMovement(horizontalInput);
        Anime();
    }

    public override void Exit()
    {
        
    }
    private void Anime()
    {
        character.animator.SetFloat("horizontalSpeed", Mathf.Abs(horizontalInput));
    }

    public override void HandlePhysics()
    {
        character.rgb2D.linearVelocity = new(horizontalInput * moveSpeed, character.rgb2D.linearVelocityY);
    }
    public override void Initialize(StateMachine machine, Mob character)
    {
        base.Initialize(machine, character);
        
    }
}
