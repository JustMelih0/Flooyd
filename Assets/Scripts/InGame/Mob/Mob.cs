using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Mob : MonoBehaviour
{
    public float facingRight = 1;
    public bool isDie = false;

    public Animator animator {get; private set;}


    protected virtual void Awake() {
        animator = GetComponent<Animator>();
    }
    public void FaceToMovement(float moveDir)
    {
        if (facingRight == 1 && moveDir < 0 || facingRight == -1 && moveDir > 0)
        {
            MobFlip();
        }
    }
    private void MobFlip()
    {
        facingRight *= -1;
        transform.localRotation = Quaternion.Euler(0, facingRight == 1 ? 0 : 180, 0);
    }
    public Collider2D CheckCircleArea(Vector2 checkPoint, float radius, LayerMask checkLayer)
    {
        return Physics2D.OverlapCircle(checkPoint, radius, checkLayer);
    }
}
