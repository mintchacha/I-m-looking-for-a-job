using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackRange : MonoBehaviour
{
    BoxCollider2D attackCollider;
    [Header("배틀 컴포넌트")]
    [SerializeField] EnemyManager enemyManager;
    UnitState unitstate;

    [Header("공격 감지 거리 offsetGizmo는 적용x")]
    public float horizontalAttackRange;
    public float verticalAttackRange;
    public float offsetX;

    Vector3 offset;

    [Header("공격 대상 설정")]
    [SerializeField] LayerMask targetLayer;

    [Header("디버깅모드")]
    [SerializeField] bool debugMode = false;

    public bool isAttack { get; private set; }

    private void Awake()
    {
        if (enemyManager == null)
        {
            Debug.Log("[AttackRange] EnemyManager 할당되지 않음");
            return;
        }
        attackCollider = GetComponent<BoxCollider2D>();
        if (attackCollider == null) 
        {
            Debug.Log("[AttackRange] attackCollider 할당되지 않음");
            return;
        }
        unitstate = enemyManager.GetComponent<UnitState>();
        if (unitstate == null)
        {
            Debug.Log("[AttackRange] unitstate 할당되지 않음");
            return;
        }

        // 콜라이더 세팅
        Vector2 size = attackCollider.size;
        size.y = verticalAttackRange;
        size.x = horizontalAttackRange;
        attackCollider.size = size;
        attackCollider.offset = offset;
    }
    private void Update()
    {
        if (debugMode) Debug.Log("적 공격범위 : " + isAttack);

        float newOffset = unitstate.direction == DIRECTION.RIGHT ? Mathf.Abs(offsetX) : -Mathf.Abs(offsetX);
        Vector3 offset = attackCollider.offset;
        offset.x = newOffset;
        attackCollider.offset = offset;
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
        if (debugMode) GizmoDrow();
    }

    void GizmoDrow()
    {
        attackCollider = GetComponent<BoxCollider2D>();
        Vector3 offset = attackCollider.offset;
        offset.x = offsetX;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, new Vector2(horizontalAttackRange * transform.lossyScale.x, verticalAttackRange * transform.lossyScale.y));

    }
}
