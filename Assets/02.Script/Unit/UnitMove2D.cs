using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitMove2D : MonoBehaviour
{
    Rigidbody2D unitRb;
    [SerializeField] UnitChase unitChase;
    [SerializeField] UnitState unitState;
    [SerializeField] AttackRange attackRange;

    [Header("이동속도")]
    public float moveSpeed = 5f;
    public Vector2 moveVector { get; private set; }

    private void Awake()
    {
        if(unitRb == null) unitRb = GetComponent<Rigidbody2D>();
        
        if (moveSpeed <= 0) {
            Debug.Log("[UnitMove2D] moveSpeed가 0 이하로 설정됨");
            return;
        }
        if (unitChase == null) {
            Debug.Log("[UnitMove2D] unitChase가 할당되지 않음");
            return;
        }
        if (unitState == null) {
            Debug.Log("[UnitMove2D] UnitState 할당되지 않음");
            return;
        }
        if (attackRange == null) {
            Debug.Log("[UnitMove2D] attackRange 할당되지 않음");
            return;
        }
    }
    private void Update()
    {
        UpdateDirection();
        UpdateState();
    }
    private void FixedUpdate()
    {
        if (unitState.state == UNITSTATE.MOVE && unitChase.isChasing) 
            unitRb.linearVelocity = moveVector * moveSpeed;        
    }

    void UpdateDirection()
    {
        // 공격범위거나  추적 시 이동 방어
        //if (!unitChase.isChasing || attackRange.isAttack) 
        if (!unitChase.isChasing) 
        {
            moveVector = Vector2.zero;
            return;
        }

        // 공격중에 방향전환방어
        if (unitState.state == UNITSTATE.ATTACK) return;

        if (unitChase.targetPosition.x > transform.position.x)
        {
            moveVector = Vector2.right;
            unitState.SetDirection(DIRECTION.RIGHT);
        }
        else if (unitChase.targetPosition.x < transform.position.x)
        {
            moveVector = Vector2.left;
            unitState.SetDirection(DIRECTION.LEFT);
        }
    }
    void UpdateState()
    {
        // 사망시엔 상태변경금지 
        if (unitState.state == UNITSTATE.DIE) return;
        // 피격시 상태변경금지
        if (unitState.state == UNITSTATE.DAMAGED)  return;

        // 공격중 아닐 시에 적용
        if (unitState.state != UNITSTATE.ATTACK && moveVector == Vector2.zero)
        {
            unitState.SetUnitState(UNITSTATE.IDLE);
        }
        else if (unitState.state != UNITSTATE.ATTACK && moveVector != Vector2.zero)
        {
            unitState.SetUnitState(UNITSTATE.MOVE);
        }
    }

}
