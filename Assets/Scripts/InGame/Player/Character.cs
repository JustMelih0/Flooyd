using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Character : Mob
{
    [Header("For Landing")]
    [SerializeField]private GameObject footPoint;
    [SerializeField]private LayerMask footLayer;
    [SerializeField]private float footRadius = 0.2f;

    public Rigidbody2D rgb2D{get; private set;}
    public Animator animator{get; private set;}

    public float facingRight = 1;

    void Awake()
    {
        rgb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void FaceToMovement(float moveDir)
    {
        if (facingRight == 1 && moveDir < 0 || facingRight == -1 && moveDir > 0)
        {
            CharacterFlip();
        }
    }
    private void CharacterFlip()
    {
        facingRight *= -1;
        transform.localRotation = Quaternion.Euler(0, facingRight == 1 ? 0 : 180, 0);
    }
    public bool IsGrounded()
    {
        return CheckCircleArea(footPoint.transform.position, footRadius, footLayer);
    }
    public Collider2D CheckCircleArea(Vector2 checkPoint, float radius, LayerMask checkLayer)
    {
        return Physics2D.OverlapCircle(checkPoint, radius, checkLayer);
    }
}
