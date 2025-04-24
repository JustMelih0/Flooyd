using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorMob_Health : Mob_Health
{
    [SerializeField] private Image healthBarFiller;  
    [SerializeField] private Image healthBarSmoother; 
    [SerializeField] private GameObject healthBarBorder;

    private Coroutine smootherCoroutine;

    protected override void Awake()
    {
        base.Awake();
        healthBarFiller.fillAmount = 1; 
        healthBarSmoother.fillAmount = 1;
    }

    public override void DamageImplemented(int damage, int direction)
    {
        base.DamageImplemented(damage, direction);

        healthBarFiller.fillAmount = (float)currentHealth / totalHealth;

        if (smootherCoroutine != null) StopCoroutine(smootherCoroutine);
        smootherCoroutine = StartCoroutine(SmoothHealthBarTransition());
    }

    private IEnumerator SmoothHealthBarTransition()
    {
        float targetHealth = (float)currentHealth / totalHealth;
        float currentFillAmount = healthBarSmoother.fillAmount;

        float transitionDuration = 0.5f; 
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = elapsedTime / transitionDuration;
            
            healthBarSmoother.fillAmount = Mathf.Lerp(currentFillAmount, targetHealth, lerpValue);

            yield return null;
        }

        healthBarSmoother.fillAmount = targetHealth;

        Debug.Log("b");
    }
}
