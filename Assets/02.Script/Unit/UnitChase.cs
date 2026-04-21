using UnityEngine;

public class UnitChase : MonoBehaviour
{
    [SerializeField] GameObject player;
    BoxCollider2D hitBoxCollider;
    [Header("추적종료거리")]
    [SerializeField] float verticalSize;
    [SerializeField] float horizontalSize;

    public Vector2 targetPosition { get; private set; }

    public bool isChasing { get; private set; }

    [SerializeField] bool debugMode = false;

    private void Start()
    {
        isChasing = true;
    }
    private void Update()
    {
        targetPosition = player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null) return;
        if (collision.gameObject == player.gameObject)
        {
            isChasing = false;
            if (debugMode) Debug.Log($"[UnitChase] 플레이어와 충돌: isChasing = {isChasing}");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player == null) return;
        if (collision.gameObject == player.gameObject)
        {
            isChasing = true;
            if (debugMode) Debug.Log($"[UnitChase] 플레이어와 충돌 해제: isChasing = {isChasing}");
        }
    }

    public void Initialize(GameObject target)
    {
        this.player = target;
        if (target == null)
        {
            Debug.LogError($"[UnitChase] Initialize 타겟설정이 올바르지 않습니다.");
            return;
        }
    }
    private void OnDrawGizmos()
    {
        hitBoxCollider = GetComponent<BoxCollider2D>();
        if (hitBoxCollider == null)
        {
            Debug.LogError($"[UnitMove2D] BoxCollider2D 컴포넌트가 할당되지 않음.");
            return;
        }
        Vector2 size = hitBoxCollider.size;
        size.y = verticalSize;
        size.x = horizontalSize;
        hitBoxCollider.size = size;

        if (debugMode) GizmoDrow();
    }

    void GizmoDrow()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(horizontalSize * transform.lossyScale.x, verticalSize * transform.lossyScale.y));
    }
}
