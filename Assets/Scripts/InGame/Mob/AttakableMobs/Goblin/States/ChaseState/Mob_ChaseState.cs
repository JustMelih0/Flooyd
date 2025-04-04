using UnityEngine;

[CreateAssetMenu(fileName = "newChaseState", menuName = "StateMachines/Mobs/Melee/ChaseState", order = 0)]
public class Mob_ChaseState : Enemies_State
{

    protected AttackableNPCBase humonoidMob;
    protected Collider2D targetObject;

    [SerializeField]protected float moveSpeed;
    public float viewRange = 1f;

    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        humonoidMob = mob as AttackableNPCBase;
    }
    public override void Enter()
    {
        mob.animator.SetTrigger("locomotionState");
        mob.animator.SetBool("isWalking", true);
    }

    public override void Execute()
    {
        machine.MachineProcess();
        targetObject = humonoidMob.IsEnemyInViewRange(viewRange);

        if(targetObject == null) return;

        float direction = targetObject.transform.position.x - mob.transform.position.x;
        direction = Mathf.Clamp(direction, -1, 1);
        humonoidMob.FaceToMovement(direction);
        humonoidMob.MoveToTarget(targetObject.transform.position, moveSpeed);
    }

    public override void Exit()
    {
        mob.animator.SetBool("isWalking", false);
    }

    public override void HandlePhysics()
    {
        
    }
    public override void GizmosDraw()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(humonoidMob.viewPoint.position, viewRange);
    }
}
