using UnityEngine;

public class CameraMove2D : MonoBehaviour
{
    [Header("카메라 추적 대상")]
    [SerializeField] Transform target;
    public float positionY = 1f;
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        if (target == null)
        {
            Debug.Log("[CameraMove2D] target 설정 안되어있음");
            return;
        }
    }
    private void LateUpdate()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y + positionY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
}
