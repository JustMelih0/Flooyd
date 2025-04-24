using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newChaseState", menuName = "StateMachines/Mobs/Melee/ChaseState", order = 0)]
public class Mob_ChaseState : Enemies_State
{

    protected AttackableNPCBase humonoidMob;
    protected Collider2D targetObject;

    [SerializeField]protected float moveSpeed;
    public float chaseTime = 5f;
    [HideInInspector]public bool chaseLimitDone = false;
    public float viewRange = 1f;

    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        humonoidMob = mob as AttackableNPCBase;
    }
    public override void Enter()
    {
        chaseLimitDone = false;
        mob.animator.SetTrigger("locomotionState");
        mob.animator.SetBool("isWalking", true);
        chaseCoroutine = mob.StartCoroutine(ChaseTimer());
    }
    protected Coroutine chaseCoroutine;
    protected IEnumerator ChaseTimer()
    {
        yield return new WaitForSeconds(chaseTime);
        chaseCoroutine = null;
        chaseLimitDone = true;
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
        if (chaseCoroutine != null)
        {
            mob.StopCoroutine(chaseCoroutine);
            chaseCoroutine = null;
        }
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
