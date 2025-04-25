using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField]protected float projectileSpeed = 1;
    [SerializeField]protected short facingRight = 1;
    [SerializeField]protected int damage = 1;
    public bool onTheTarget;
    protected Vector2 targetPos;
    protected GameObject mainObject;

    public void SetTargetPos(Vector2 targetPos, bool doRotate)
    {
        this.targetPos = targetPos;
        if(doRotate) FaceToTarget(targetPos.x);
        if (goToTargetCoroutine != null)
        {
            StopCoroutine(goToTargetCoroutine);
        }
        goToTargetCoroutine = StartCoroutine(GoToTargetCoroutine());
    }
    public void InitializeProjectile(GameObject mainObject, Vector2 targetPos, float speed, int damage)
    {
        SetTargetPos(targetPos, true);
        projectileSpeed = speed;
        this.mainObject = mainObject;
        this.damage = damage;
    }
    Coroutine goToTargetCoroutine;
    protected IEnumerator GoToTargetCoroutine()
    {
        onTheTarget = false;
        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * projectileSpeed);
            yield return null;
        }
        onTheTarget = true;
        goToTargetCoroutine = null;
    }

    public void FaceToTarget(float targetX)
    {
        if (facingRight == 1 && transform.position.x > targetX || facingRight == -1 && transform.position.x < targetX)
        {
            ProjectileFlip();
        }
    }
    private void ProjectileFlip()
    {
        facingRight *= -1;
        transform.localRotation = Quaternion.Euler(0, facingRight == 1 ? 0 : 180, 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(mainObject != null && mainObject == collision.gameObject) return;

        if (collision.TryGetComponent(out IHitable hitable))
        {
            hitable.TakeDamage(damage, facingRight, false);
        }
    }
}
