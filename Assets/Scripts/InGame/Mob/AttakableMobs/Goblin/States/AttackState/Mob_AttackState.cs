using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackState", menuName = "StateMachines/Mobs/Melee/AttackState", order = 0)]
public class Mob_AttackState : Enemies_State
{
    public float attackRadius = 1f;
    public float attackViewRadius = 3f;
    public float attackCoolDown = 0.5f;
    public float attackForce = 5f;
    Collider2D targetObject;
    AttackableNPCBase attackableNPC;
    protected Coroutine coolDownCoroutine;

    public override void Enter()
    {
        mob.animator.SetTrigger("attackState");
        machine.canTransitionState = false;
        coolDownCoroutine = mob.StartCoroutine(AttackCoolDown());
    }

    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        attackableNPC = mob as AttackableNPCBase;

    }
    public override void Execute()
    {
    }

    public override void OnAnimationStarted()
    {
        attackableNPC.rgb2D.AddForce(mob.facingRight * attackForce * Vector2.right, ForceMode2D.Impulse);

        targetObject = attackableNPC.IsEnemyInViewRange(attackViewRadius);

        if(targetObject == null) return;

        attackableNPC.FaceToTarget(targetObject.transform.position.x);
    }

    public override void OnAnimation()
    {
        targetObject = attackableNPC.IsEnemyInAttackRange(attackRadius);


        if(targetObject == null) return;

        Debug.Log($"{targetObject} hasar vuruldu");
    }

    public override void OnAnimationEnded()
    {
        if(coolDownCoroutine != null)
        {
            mob.StopCoroutine(coolDownCoroutine);
            coolDownCoroutine = null;
        }
        machine.canTransitionState = true;
        machine.MachineProcess();
    }

    public override void Exit()
    {
       
    }

    public override void HandlePhysics()
    {
       
    }
    public override void GizmosDraw()
    {
        base.GizmosDraw();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackableNPC.attackPoint.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackableNPC.viewPoint.position, attackViewRadius);
    }

    protected IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCoolDown);
        coolDownCoroutine = null;
        mob.animator.SetTrigger("attack");
        
    }
}
