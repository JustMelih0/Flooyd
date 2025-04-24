using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleState", menuName = "StateMachines/Mobs/Melee/IdleState", order = 0)]
public class Mob_IdleState : Enemies_State
{
    public float viewRange = 1f;
    public float lossTime = 0.5f;
    private bool isReturning = false;
    public float returnSpeed = 2f;
    public float sleepTime = 5f;
    public bool canSleep = false;
    Vector2 startPos;

    protected Coroutine lossCoroutine;
    protected Coroutine sleepCoroutine;
    protected AttackableNPCBase humonoidMob;
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        humonoidMob = mob as AttackableNPCBase;
        startPos = mob.transform.position;
    }
    public override void Enter()
    {
        isReturning = false;
        mob.animator.SetBool("isWalking", isReturning);
        if(canSleep) mob.animator.SetBool("isSleeping", false);
        mob.animator.SetTrigger("locomotionState");
        if (Mathf.Abs(startPos.x - mob.transform.position.x) >= 0.1f) lossCoroutine = mob.StartCoroutine(LossCoroutine()); 
        else if(canSleep) sleepCoroutine = mob.StartCoroutine(SleepCoroutine());
    }
    
    public override void Execute()
    {
        machine.MachineProcess();
        if(isReturning)
        {
            float direction = startPos.x - mob.transform.position.x;
            direction = Mathf.Clamp(direction, -1, 1);
            humonoidMob.FaceToMovement(direction);
            humonoidMob.MoveToTarget(startPos, returnSpeed);
            if (Mathf.Abs(startPos.x - mob.transform.position.x) <= 0.1f)
            {
                isReturning = false;
                mob.animator.SetBool("isWalking", isReturning);
                if(canSleep) sleepCoroutine = mob.StartCoroutine(SleepCoroutine());
            }       
        }

    }

    public override void Exit()
    {
        if (lossCoroutine != null)
        {
            mob.StopCoroutine(lossCoroutine);
            lossCoroutine = null;
        }

        if (sleepCoroutine != null)
        {
            mob.StopCoroutine(sleepCoroutine);
            sleepCoroutine = null;
        }
        if(canSleep) mob.animator.SetBool("isSleeping", false);
    }

    public override void HandlePhysics()
    {
        
    }
    public override void GizmosDraw()
    {
        base.GizmosDraw();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(humonoidMob.viewPoint.position, viewRange);
    }
    protected IEnumerator LossCoroutine()
    {
        yield return new WaitForSeconds(lossTime);
        lossCoroutine = null;
        isReturning = true;
        mob.animator.SetBool("isWalking", isReturning);
    }
    protected IEnumerator SleepCoroutine()
    {
        yield return new WaitForSeconds(sleepTime);
        sleepCoroutine = null;
        mob.animator.SetBool("isSleeping", true);
    }


}
