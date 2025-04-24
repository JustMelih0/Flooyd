using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterHealth))]
public class Character : Mob
{
    [Header("For Landing")]
    [SerializeField] private Transform footPoint;
    [SerializeField] private LayerMask footLayer;
    [SerializeField] private float footRadius = 0.2f;

    [Header("For Combat")]
    public Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackLayer;

    public Rigidbody2D rgb2D { get; private set; }
    public CharacterHealth characterHealth {get; private set;}


    protected override void Awake()
    {
        base.Awake();
        rgb2D = GetComponent<Rigidbody2D>();
        characterHealth = GetComponent<CharacterHealth>();
    }
    
    public Collider2D IsGrounded()
    {
        return CheckCircleArea(footPoint.transform.position, footRadius, footLayer);
    }
    public Collider2D IsEnemyInAttackRange()
    {
        return CheckCircleArea(attackPoint.transform.position, attackRadius, attackLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(footPoint.transform.position, footRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
