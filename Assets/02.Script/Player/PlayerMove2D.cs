using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove2D : MonoBehaviour
{
    [SerializeField] PlayerInputReader inputReader; 
    [SerializeField] UnitState unitState;
    Rigidbody2D playerRb;
    // 벽 부딫힘 감지 레이어
    LayerMask wallLayer;

    [Header("이동속도")]
    public float moveSpeed = 5f;
    [Header("점프력")]
    public float jumpForce = 15f;

    bool isGrounded;

    [SerializeField] bool debugMode = false;

    private void Awake()
    {
        if (inputReader == null)
        {
            Debug.Log("[PlayerMove2D] PlayerInputReader 참조되지 않음.");
            return;
        }
        if (unitState == null)
        {
            Debug.Log("[PlayerMove2D] UnitState 참조되지 않음.");
            return;
        }
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        unitState.SetUnitState(UNITSTATE.IDLE);
        wallLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        // 좌우 상태 감지
        UpdateDirection();
        UpdateState();
    }

    private void FixedUpdate()
    {
        if (inputReader.jumpPressed) Jump();

        Vector2 newPosition = inputReader.MoveVector * moveSpeed;

        // 공격중엔, 사망 시 이동 금지 x0으로 안하면 이전 물리 남아서 미끄러짐           
        if (unitState.state == UNITSTATE.DIE)
        {
            playerRb.linearVelocity = new Vector2(0f, 0f);
        }
        else if (unitState.state == UNITSTATE.ATTACK)
        {
            playerRb.linearVelocity = new Vector2(0f, playerRb.linearVelocity.y);
        }
        else 
        {
            playerRb.linearVelocityX = newPosition.x;
        }

        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            isGrounded = true;
        }
    }

    // 방향 업데이트
    void UpdateDirection()
    {
        // 공격중, 사망시엔 방향변경금지 
        if (BattleManager.isAttack || unitState.state == UNITSTATE.DIE) return;

        if (inputReader.MoveVector.x > 0.1f)
        {
            unitState.SetDirection(DIRECTION.RIGHT);
        }
        else if (inputReader.MoveVector.x < -0.1f)
        {
            unitState.SetDirection(DIRECTION.LEFT);            
        }
        if (debugMode) Debug.Log("플레이어 방향 : " + unitState.direction);
    }


    void UpdateState()
    {
        // 사망시엔 상태변경금지 
        if (unitState.state == UNITSTATE.DIE) return;       

        if (!isGrounded)
        {
            unitState.SetUnitState(UNITSTATE.JUMP);
            return;
        }
        // 공격중 아닐 시에 적용
        if (unitState.state != UNITSTATE.ATTACK && inputReader.MoveVector == Vector2.zero)
        {
            unitState.SetUnitState(UNITSTATE.IDLE);
        }
        else if (unitState.state != UNITSTATE.ATTACK && inputReader.MoveVector != Vector2.zero)
        {
            unitState.SetUnitState(UNITSTATE.MOVE);
        }
    }

    // InputReader에서 update 프레임에 점프 키 true 로 들어온 순간 실행될 함수
    public void Jump()
    {
        // 연속으로 입력하면 JumpPressed 가 다시 true 되어 무조건 false로 리셋되도록 작성
        inputReader.ResetJump();
        BattleManager.SetAttackReset(); // 점프하면 공격상태 해제
        
        if (!isGrounded) return;
        playerRb.AddForceY(jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }
}
