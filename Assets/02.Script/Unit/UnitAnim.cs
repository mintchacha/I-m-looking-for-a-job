using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnim : MonoBehaviour, IUnitAnim
{
    [SerializeField] UnitHealth unitHealth;
    [SerializeField] UnitState unitState;
    [SerializeField] UnitMove2D move2D;
    [SerializeField] GameObject owner;
    [SerializeField] EnemyManager enemyManager;

    Animator unitAnim;
    string unitDirectionName = "Direction";
    int unitDirectionHash;
    string isMovingParamName = "IsMoving";
    int isMovingHash;
    string damageTriggerName = "IsDamaged";
    int damageTriggerHash;
    string dieTriggerName = "IsDie";
    int dieTriggerHash;
    string attackTriggerName = "IsAttack";
    int attackTriggerHash;

    bool isMoving = false;

    private void Awake()
    {
        if(unitAnim == null) unitAnim = GetComponent<Animator>();
        if (unitHealth == null) 
        {
            Debug.Log("[UnitAnim] unitHealth가 참조되지 않음.");
            return;
        }
        if (unitAnim == null) 
        {
            Debug.Log("[UnitAnim] UnitMove2D가 참조되지 않음.");
            return;
        }
        enemyManager.AttackTrigger += OnUnitAttack;

        unitDirectionHash = Animator.StringToHash(unitDirectionName);
        isMovingHash = Animator.StringToHash(isMovingParamName);
        damageTriggerHash = Animator.StringToHash(damageTriggerName);
        dieTriggerHash = Animator.StringToHash(dieTriggerName);
        attackTriggerHash = Animator.StringToHash(attackTriggerName);
    }
    private void OnDestroy()
    {
        enemyManager.AttackTrigger -= OnUnitAttack;
    }
    private void Update()
    {
        isMoving = move2D.moveVector != Vector2.zero ? true : false;
    }
    private void LateUpdate()
    {
        float direction = (unitState.direction == DIRECTION.LEFT) ? 0f : 1f;
        unitAnim.SetFloat(unitDirectionHash, direction);
        unitAnim.SetBool(isMovingHash, isMoving);
    }

    // TakeDamage 이벤트가 발생할 때마다 inputDamage를 true로 설정하는 메서드
    public void OnDamageTaken()
    {
        unitAnim.SetTrigger(damageTriggerHash);
    }
    public void OnUnitAttack()
    {
        unitAnim.SetTrigger(attackTriggerHash);
    }
    public void OnDie()
    {
        unitAnim.SetBool(dieTriggerHash, (unitState.state == UNITSTATE.DIE));
    }
    // 공격 애니메이션 후 실행
    void SpawnHitBox() 
    {
        enemyManager.spawnHitBox();
    }

    // 사망 애니메이션 후 실행
    void UnitDestroy() => Destroy(owner);

}
