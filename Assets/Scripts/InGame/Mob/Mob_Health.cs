using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public abstract class Mob_Health : MonoBehaviour, IHitable
{
    [SerializeField] protected int totalHealth = 1;
    [SerializeField] protected Material impactMaterial;
    protected Material defaultMaterial;
    public bool canDodge = false;

    protected int currentHealth;
    protected Rigidbody2D rgb2d;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]protected Mob mob;

    protected virtual void Awake()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mob = GetComponent<Mob>();
    }
    protected virtual void Start() {
        currentHealth = totalHealth;
        defaultMaterial = spriteRenderer.material;
        canDodge = false;
    }

    public virtual void TakeDamage(int damage, int direction, bool trueDamage)
    {
        if (DodgeControl(direction) && !trueDamage) DamageDodged();
        else DamageImplemented(damage, direction);
    }
    public virtual void DamageDodged()
    {

    }
    protected virtual bool DodgeControl(int direction)
    {
        if (!canDodge || mob.facingRight == direction) return false;

        return true;
    }
    public virtual void DamageImplemented(int damage, int direction)
    {

        rgb2d.AddForce(direction * Vector2.right, ForceMode2D.Impulse);
        impactCoroutine ??= StartCoroutine(ImpactEffect());
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, totalHealth);
        if(currentHealth == 0 ) MobDead();

    }
    protected Coroutine impactCoroutine;
    public virtual IEnumerator ImpactEffect()
    {
        spriteRenderer.material = impactMaterial;

        yield return new WaitForSeconds(0.1f);

        impactCoroutine = null;
        spriteRenderer.material = defaultMaterial;
    }

    protected virtual void MobDead()
    {
        mob.isDie = true;
        //Destroy(gameObject);
    }
}
