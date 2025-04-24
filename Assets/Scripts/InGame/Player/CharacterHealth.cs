using System;
using System.Collections;
using UnityEngine;

public class CharacterHealth : IndicatorMob_Health
{
    public event Action BlockedAttackAction;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void DamageDodged()
    {
        base.DamageDodged();
        BlockedAttackAction?.Invoke();
    }
    public override void DamageImplemented(int damage, int direction)
    {
        CameraController.Instance.Shake();
        base.DamageImplemented(damage, direction);
    }
    public override IEnumerator ImpactEffect()
    {
        return base.ImpactEffect();
    }
    public override void TakeDamage(int damage, int direction, bool trueDamage)
    {
        CameraController.Instance.Shake();
        base.TakeDamage(damage, direction, trueDamage);
    }
}
