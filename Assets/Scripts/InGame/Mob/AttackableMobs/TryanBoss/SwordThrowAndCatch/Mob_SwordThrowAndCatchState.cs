using UnityEngine;

[CreateAssetMenu(fileName = "new_SwordThrowState", menuName = "StateMachines/Mobs/TyranBoss/SwordThrowState", order = 0)]
public class Mob_SwordThrowAndCatchState : Enemies_State
{
    [SerializeField]protected ProjectileBase projectileInstance;
    [SerializeField]protected float viewRadius = 20;
    [SerializeField]protected float attackRadius = 0.5f;
    [SerializeField]protected float projectileSpeed = 1f;
    [SerializeField]protected int damage = 1;
    [SerializeField]protected float attackForce = 1f;
    [SerializeField]protected float farDistance = 5f;
    [SerializeField]protected float projectileBehindOffset = 1f;
    protected bool throwProjectile = true;
    public bool isEnemyFarEnough => humonoidMob.IsEnemyFarEnough(farDistance, viewRadius);

    ProjectileBase projectile;
    protected AttackableNPCBase humonoidMob;
    protected Collider2D target;
    public override void Enter()
    {
        target = humonoidMob.IsEnemyInViewRange(viewRadius);
        if (target == null)
        {
            machine.MachineProcess();
            return;
        }
        throwProjectile = true;
        machine.canTransitionState = false;
        humonoidMob.animator.SetTrigger("swordThrowState");
    }
    public override void Initialize(StateMachine machine, Mob mob)
    {
        base.Initialize(machine, mob);
        humonoidMob = mob as AttackableNPCBase;
    }
    public override void OnAnimationStarted()
    {
        humonoidMob.rgb2D.AddForce(mob.facingRight * attackForce * Vector2.right, ForceMode2D.Impulse);

        if(!throwProjectile) return;


        Vector2 targetPos = target.transform.position;

        float direction = Mathf.Sign(targetPos.x - humonoidMob.transform.position.x);

        targetPos.x += direction * projectileBehindOffset;
        targetPos.y = humonoidMob.attackPoint.position.y + 0.2f;
        humonoidMob.FaceToTarget(target.transform.position.x);
        projectile = Instantiate(projectileInstance, humonoidMob.attackPoint.position, Quaternion.identity);
        projectile.InitializeProjectile(humonoidMob.gameObject, targetPos, projectileSpeed, damage);
    }
    public override void OnAnimation()
    {
        Collider2D attackTarget = humonoidMob.IsEnemyInAttackRange(attackRadius);
        if (attackTarget != null && attackTarget.TryGetComponent(out IHitable hitable))
        {
            hitable.TakeDamage(damage, humonoidMob.facingRight, false);
        }
    }
    public override void OnAnimationEnded()
    {
        machine.canTransitionState = true;
        machine.MachineProcess();
    }
    public override void Execute()
    {
        if (projectile == null) return;

        if (projectile.onTheTarget && throwProjectile == false)
        {
            humonoidMob.animator.SetTrigger("swordCatchAttack");
            Destroy(projectile.gameObject);
            projectile = null;
        }
        else if (projectile.onTheTarget && throwProjectile)
        {
            Vector2 targetPos = humonoidMob.attackPoint.transform.position;
            targetPos.y = humonoidMob.attackPoint.position.y + 0.2f;
            projectile.InitializeProjectile(humonoidMob.gameObject, targetPos, projectileSpeed, damage);
            throwProjectile = false;  
        }
    }

    public override void Exit()
    {
        
    }

    public override void HandlePhysics()
    {
        
    }
}
