using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "DashAttackState", menuName = "StateMachines/Mobs/WolfBoss/DashAttackState", order = 0)]
public class Mob_DashState : Enemies_State
{
    [SerializeField]protected float dashForce = 2f;
    [SerializeField]protected float dashTime = 1;
    [SerializeField]protected float dashAttackRadius = 0.3f;
    [SerializeField]protected float dashMainWievRadius = 1f;
    [SerializeField]protected int dashDamage = 2;
    protected AttackableNPCBase attackableMob;
    protected bool isDashing = false;
    public override void Enter()
    {
        
        machine.canTransitionState = false;
        mob.animator.SetBool("dashEnd", false);
        attackableMob.rgb2D.linearVelocityX = 0;
        
        Collider2D target = attackableMob.IsEnemyInViewRange(dashMainWievRadius);

        if (target != null) attackableMob.FaceToTarget(target.transform.position.x);
        mob.animator.SetTrigger("dashState");

    }
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        attackableMob = mob as AttackableNPCBase;
    }
    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        if (dashCoroutine != null)
        { 
            mob.StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }
        attackableMob.rgb2D.linearVelocityX = 0;

    }

    public override void HandlePhysics()
    {
        if (isDashing)
        {
            attackableMob.rgb2D.linearVelocityX = dashForce * mob.facingRight;
            Collider2D target = attackableMob.IsEnemyInViewRange(dashAttackRadius);
            if (target != null )
            {
                if (target.TryGetComponent(out IHitable hitable))
                {
                    hitable.TakeDamage(dashDamage, mob.facingRight, false);
                    isDashing = false;
                    if (dashCoroutine != null)
                    {
                        mob.StopCoroutine(dashCoroutine);
                        dashCoroutine = null;
                    }
                    mob.animator.SetBool("dashEnd", true);
                }
            }
        }
    }

    protected Coroutine dashCoroutine;
    protected IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashTime);
        dashCoroutine = null;
        //isDashing = false;
        mob.animator.SetBool("dashEnd", true);
    }
    public override void OnAnimationStarted()
    {
        isDashing = true;
        dashCoroutine = mob.StartCoroutine(DashCoroutine());
    }
    public override void OnAnimation()
    {

    }
    public override void OnAnimationEnded()
    {
        machine.canTransitionState = true;
        machine.MachineProcess();
    }
    public override void GizmosDraw()
    {
        base.GizmosDraw();
        Gizmos.DrawWireSphere(attackableMob.viewPoint.position, dashAttackRadius);
        Gizmos.DrawWireSphere(attackableMob.viewPoint.position, dashMainWievRadius);
    }
}
