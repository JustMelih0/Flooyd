using UnityEngine;

[CreateAssetMenu(fileName = "new_TeleportAttackState", menuName = "StateMachines/Mobs/TyranBoss/TeleportAttackState", order = 0)]
public class Mob_TeleportAttackState : Enemies_State
{
    [SerializeField]protected float viewRadius = 10f;
    [SerializeField]protected float attackRadius = 0.5f;
    [SerializeField]protected int attackDamage = 1;
    [SerializeField]protected float attackForce = 1f;

    [SerializeField]protected float farDistance = 5f;

    public bool isEnemyFarEnough => humonoidMob.IsEnemyFarEnough(farDistance, viewRadius);
    protected bool isTeleported = false;
    protected Collider2D target;
    protected AttackableNPCBase humonoidMob;
    public override void Enter()
    {
        target = humonoidMob.IsEnemyInViewRange(viewRadius);
        if (target == null)
        {
            machine.MachineProcess();
            return;
        }
        isTeleported = false;
        machine.canTransitionState = false;
        humonoidMob.animator.SetTrigger("teleportAttackState");

        Collider2D targetObject = humonoidMob.IsEnemyInViewRange(viewRadius);

        if(targetObject == null) return;

        humonoidMob.FaceToTarget(targetObject.transform.position.x);
    }
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        this.humonoidMob = mob as AttackableNPCBase;
    }
    public override void OnAnimationStarted()
    {
        Collider2D targetObject = humonoidMob.IsEnemyInViewRange(viewRadius);

        if(targetObject == null) return;

        humonoidMob.FaceToTarget(targetObject.transform.position.x);

        if(!isTeleported)
        {
            TpToTarget();
            return;
        } 

        humonoidMob.rgb2D.AddForce(mob.facingRight * attackForce * Vector2.right, ForceMode2D.Impulse);


    }
    protected void TpToTarget()
    {

        Vector3 targetPosition = target.transform.position;
        Vector3 mobPosition = humonoidMob.transform.position;


        float distanceX = targetPosition.x - mobPosition.x;

        float teleportOffset = 1.5f; 

        float direction = Mathf.Sign(distanceX);

        Vector3 teleportPosition = new(
            targetPosition.x - (teleportOffset * direction), 
            mobPosition.y, 
            mobPosition.z
        );

        humonoidMob.transform.position = teleportPosition;
        humonoidMob.FaceToTarget(target.transform.position.x);
        isTeleported = true;
    }
    public override void OnAnimation()
    {
        Collider2D attackTarget = humonoidMob.IsEnemyInAttackRange(attackRadius);
        if (attackTarget != null && attackTarget.TryGetComponent(out IHitable hitable))
        {
            hitable.TakeDamage(attackDamage, humonoidMob.facingRight, false);
        }
    }
    public override void OnAnimationEnded()
    {
        //finish to state
        machine.canTransitionState = true;
        machine.MachineProcess();
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
    public override void GizmosDraw()
    {
        base.GizmosDraw();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(humonoidMob.viewPoint.position, viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(humonoidMob.attackPoint.position, attackRadius);
    }
}
