using UnityEngine;

// TODO: do we need this
public class CameraFollow : MonoBehaviour
{
    public Transform cameraTarget;
    public float smoothSpeed = .125f;
    public Vector3 offSet;
    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 desiredPosition = cameraTarget.position + offSet;
        Vector3 smoothedPosition =
            Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);
        smoothedPosition.z = -10.0f;
        smoothedPosition.y = 0f;
        transform.position = smoothedPosition;
    }
}