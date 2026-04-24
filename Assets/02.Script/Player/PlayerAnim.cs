using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour, IUnitAnim
{
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] UnitHealth unitHealth;
    [SerializeField] UnitState unitState;
    [SerializeField] PlayerMove2D playerMove2D;

    Animator playerAnimator;

    string moveXParamName = "MoveX";
    int moveXHash;
    string isMovingParamName = "IsMoving";
    int isMovingHash;
    string directionParamName = "Direction";
    int directionHash;
    string isJumpParamName = "IsJump";
    int isJumpHash;

    // 기본공격 액션 명
    string isAttackParamName = "IsAttack";
    int isAttackHash;
    string AttackComboParamName = "AttackCombo";
    int AttackComboHash;
    string isSpecialAttackParamName = "IsSpecialAttack";
    int isSpecialAttackHash;
    string damageTriggerName = "IsDamaged";
    int damageTriggerHash;
    string dieTriggerName = "IsDie";
    int dieTriggerHash;

    // 마지막방향에 따라 대기 모션방향 설정
    float animeDirection;
    bool animeJump;

    private void Awake()
    {
        if (inputReader == null)
        {
            Debug.Log($"[PlayerAnim] PlayerInputReader 연결 안됨. {inputReader}");
            return;
        }
        if (unitState == null)
        {
            Debug.Log($"[PlayerAnim] UnitState 연결 안됨. {unitState}");
            return;
        }
        if (unitHealth == null)
        {
            Debug.Log($"[PlayerAnim] UnitHealth 연결 안됨. {unitHealth}");
            return;
        }
        if (playerAnimator == null) playerAnimator = GetComponent<Animator>();

        moveXHash = Animator.StringToHash(moveXParamName);
        isMovingHash = Animator.StringToHash(isMovingParamName);
        directionHash = Animator.StringToHash(directionParamName);
        isJumpHash = Animator.StringToHash(isJumpParamName);

        // 기본공격 여부
        isAttackHash = Animator.StringToHash(isAttackParamName);
        AttackComboHash = Animator.StringToHash(AttackComboParamName);
        isSpecialAttackHash = Animator.StringToHash(isSpecialAttackParamName);

        damageTriggerHash = Animator.StringToHash(damageTriggerName);
        dieTriggerHash = Animator.StringToHash(dieTriggerName);
    }

    private void Update()
    {
        animeDirection = unitState.direction == DIRECTION.LEFT ? 0f : 1f;    
        // 기본공격 여부
        playerAnimator.SetBool(isAttackHash, BattleManager.isAttackParam);
        playerAnimator.SetInteger(AttackComboHash, BattleManager.currentAttackCount);
        // 필살기
        playerAnimator.SetBool(isSpecialAttackHash, (unitState.state == UNITSTATE.SPECIALATTACK));

        OnDie();
    }

    void LateUpdate()
    {
        playerAnimator.SetBool(isMovingHash, (unitState.state == UNITSTATE.MOVE));
        playerAnimator.SetBool(isJumpHash, !playerMove2D.isGrounded);
        playerAnimator.SetFloat(moveXHash, inputReader.MoveVector.x);
        playerAnimator.SetFloat(directionHash, animeDirection);

    }

    public void OnDamageTaken()
    {
        playerAnimator.SetTrigger(damageTriggerHash);
    }
    public void OnDie()
    {
        playerAnimator.SetBool(dieTriggerHash, (unitState.state == UNITSTATE.DIE));
    }
    public void SetZoomOut()
    {
        CameraMove2D.Instance.SetZoomOut();
    }
    public void SetZoomIn()
    {
        CameraMove2D.Instance.SetZoomIn();
    }
    public void ResetState()
    {
        unitState.SetUnitState(UNITSTATE.IDLE);
    }

}
