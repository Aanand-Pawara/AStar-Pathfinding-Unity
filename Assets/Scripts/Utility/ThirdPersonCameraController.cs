using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector2 pitchMinMax = new Vector2(-40, 85);
    [SerializeField] private KeyCode resetKey = KeyCode.R;

    private float yaw;
    private float pitch = 0f;
    private Vector3 smoothDampVelocity;


    void LateUpdate()
    {
       
        Vector3 targetPosition = target.position - offset;

      
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

       
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel"), minDistance, maxDistance);

     
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

       
        Vector3 desiredPosition = targetPosition + rotation * Vector3.back * distance;

       
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothDampVelocity, smoothSpeed);
        transform.LookAt(targetPosition);

        if (Input.GetKeyDown(resetKey))
        {
            yaw = 0;
            pitch = 0;
            distance = 5f;
        }
    }
}
