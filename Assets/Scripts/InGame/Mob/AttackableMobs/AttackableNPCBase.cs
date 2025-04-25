using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class AttackableNPCBase : Mob
{
    public Transform attackPoint;
    public Transform viewPoint;
    public LayerMask targetLayer;

    public Rigidbody2D rgb2D {get; private set;}
    public BoxCollider2D boxCollider2D {get; private set;}
    public Mob_Health mob_Health;

    protected override void Awake()
    {
        base.Awake();
        rgb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public virtual void MoveToTarget(Vector2 targetPoint, float moveSpeed)
    {
        targetPoint.y = transform.position.y;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }
    public virtual Collider2D IsEnemyInViewRange(float radius)
    {
        return CheckCircleArea(viewPoint.position, radius, targetLayer);
    }
    public virtual Collider2D IsEnemyInAttackRange(float radius)
    {
        return CheckCircleArea(attackPoint.position, radius, targetLayer);
    }
    public bool IsEnemyFarEnough(float farDistance, float viewRadius)
    {
        Collider2D enemy = IsEnemyInViewRange(viewRadius);
        if (enemy == null)
        {
            return false;
        }

        if (Mathf.Abs(enemy.transform.position.x - transform.position.x) > farDistance)
        {
            return true;
        }
        return false;
    }


}
