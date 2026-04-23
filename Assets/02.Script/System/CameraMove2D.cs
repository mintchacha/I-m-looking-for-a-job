using UnityEngine;

public class CameraMove2D : MonoBehaviour
{
    public static CameraMove2D Instance;
    
    [Header("카메라 추적 대상")]
    [SerializeField] Transform target;
    public float positionY = 1f;
    public float positionZ = -5f;
    public float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;
    Vector3 newPosition;

    public bool isZoom { get; private set; }
    public void SetZoomIn () => isZoom = true;
    public void SetZoomOut () => isZoom = false;

    private void Awake()
    {
        if (target == null)
        {
            Debug.Log("[CameraMove2D] target 설정 안되어있음");
            return;
        }
        Instance = this;
    }
    private void LateUpdate()
    {
        FallowPlayer();
    }
    void FallowPlayer()
    {
        if (isZoom)
        {
            newPosition = new Vector3(target.position.x, target.position.y + positionY, positionZ * 0.5f);
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }
        else 
        {
            newPosition = new Vector3(target.position.x, target.position.y + positionY, positionZ);
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.1f);
        }

        
    }
}
