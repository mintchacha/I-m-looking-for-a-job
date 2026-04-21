using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackRange : MonoBehaviour
{
    BoxCollider2D attackCollider;
    [Header("배틀 컴포넌트")]
    [SerializeField] EnemyManager enemyManager;

    [Header("공격 감지 거리")]
    public float horizontalAttackRange;
    public float verticalAttackRange;

    [Header("공격 대상 설정")]
    [SerializeField] LayerMask targetLayer;

    [Header("디버깅모드")]
    [SerializeField] bool debugMode = false;

    float verticalSize;
    float horizontalSize;

    public bool isAttack { get; private set; }

    private void Awake()
    {
        if (enemyManager == null)
        {
            Debug.Log("[AttackRange] EnemyManager 할당되지 않음");
            return;
        }
    }
    private void Update()
    {
        if (debugMode) Debug.Log("적 공격범위 : " + isAttack);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 타겟대상일때면 실행
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            // 첫 접촉시부터 공격 시간 소요
            if (enemyManager.LastAttackTime < 0) enemyManager.AttackTimeInit();
            isAttack = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            UnitState target = collision.GetComponentInParent<UnitState>();
            // 공격상태 도중 타겟이 죽으면 공격상태 취소
            if (target.state == UNITSTATE.DIE)
            {
                isAttack = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {        
        if ((1 << collision.gameObject.layer & targetLayer) != 0) isAttack = false;        
    }

    private void OnDrawGizmos()
    {
        verticalSize = verticalAttackRange;
        horizontalSize = horizontalAttackRange;

        if (attackCollider == null) attackCollider = GetComponent<BoxCollider2D>();

        Vector2 size = attackCollider.size;
        size.y = verticalSize;
        size.x = horizontalSize;
        attackCollider.size = size;

        if (debugMode) GizmoDrow();
    }

    void GizmoDrow()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(horizontalSize * transform.lossyScale.x, verticalSize * transform.lossyScale.y));

    }
}
