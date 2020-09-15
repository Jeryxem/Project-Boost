using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraTarget;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        if (cameraTarget == null)
            return;

        Vector3 desiredPosition = cameraTarget.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(cameraTarget);
    }
}
