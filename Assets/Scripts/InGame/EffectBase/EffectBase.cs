using UnityEngine;

public class EffectBase : MonoBehaviour
{
    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
