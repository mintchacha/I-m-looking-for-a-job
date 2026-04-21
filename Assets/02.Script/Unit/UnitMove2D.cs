using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitMove2D : MonoBehaviour
{
    public Vector2 moveVector { get; private set; }    

    Rigidbody2D unitRb;
    [SerializeField] UnitChase unitChase;
    [SerializeField] UnitState unitState;

    [Header("이동속도")]
    public float moveSpeed = 5f;


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
    }
    private void Update()
    {
        UpdateDirection();
    }
    private void FixedUpdate()
    {
        if (unitState.state != UNITSTATE.DIE && unitChase.isChasing) unitRb.linearVelocity = moveVector * moveSpeed;        
    }

    void UpdateDirection()
    {
        if (!unitChase.isChasing) 
        {
            moveVector = Vector2.zero;
            return;
        }

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
}
