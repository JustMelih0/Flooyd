using System.Collections;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    // Singleton instance
    public static CameraController Instance { get; private set; }

    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shakeDuration = 0.5f; 
    [SerializeField] private float shakeMagnitude = 0.2f; 
    [SerializeField]private float offsetX;
    [SerializeField]private float offsetY;

    private bool isShaking = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (!isShaking && target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x + offsetX, target.position.y + offsetY, -10f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void Shake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;

        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isShaking = false;
    }
}

