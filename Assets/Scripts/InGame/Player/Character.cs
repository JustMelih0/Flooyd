using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : Mob
{
    [Header("For Landing")]
    [SerializeField] private GameObject footPoint;
    [SerializeField] private LayerMask footLayer;
    [SerializeField] private float footRadius = 0.2f;

    public Rigidbody2D rgb2D { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        rgb2D = GetComponent<Rigidbody2D>();
    }
    
    public Collider2D IsGrounded()
    {
        return CheckCircleArea(footPoint.transform.position, footRadius, footLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(footPoint.transform.position, footRadius);
    }
}
