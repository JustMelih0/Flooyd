using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxMultiplier = 0.5f;

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxMultiplier, delta.y * parallaxMultiplier, 0f);
        lastCameraPosition = cameraTransform.position;
    }
}

