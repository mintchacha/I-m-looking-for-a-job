using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [Header("유닛의 상태 컴포넌트")]
    [SerializeField] UnitStat unitStat;
    [SerializeField] UnitMove2D unitMove2D;
    [SerializeField] UnitState unitState;
    [SerializeField] AttackRange attackRange;

    [Header("히트박스 종류")]
    [SerializeField] HitBox hitBox;

    GameObject owner;
    float attackCoolDown;

    float lastAttackTime = -999f;
    public float LastAttackTime => lastAttackTime;

    [Header("공격 지속시간")]
    public float attckDuration = 0.2f;
    [Header("공격 생성 거리")]
    public float attckDistance = 0.3f;
    public Action AttackTrigger;

    //[SerializeField] bool debugMode = true;

    private void Awake()
    {
        if (unitStat == null)
        {
            Debug.Log("[BattleManager] unitStat 할당되지 않음");
            return;
        }
        if (hitBox == null)
        {
            Debug.Log("[BattleManager] hitBox가 할당되지 않음");
            return;
        }
        if (attackRange == null)
        {
            Debug.Log("[BattleManager] attackRange 할당되지 않음");
            return;
        }
        if (unitMove2D == null)
        {
            Debug.Log("[BattleManager] unitMove2D 할당되지 않음");
            return;
        }
    }
    private void OnEnable()
    {
        attackCoolDown = unitStat.statData.atkSpeed;
    }
    private void Update()
    {
        // 공격 범위 안에 들어오고, 공격 쿨타임이 경과했을때 공격 실행
        if (unitState.state != UNITSTATE.DAMAGED && attackRange.isAttack && Time.time > lastAttackTime + attackCoolDown)
        {
            EnemyAttack();
        } else if (unitState.state == UNITSTATE.ATTACK && Time.time > lastAttackTime + attckDuration)
        {
            // 공격상태이며 공격시간끝나고 
            unitState.SetUnitState(UNITSTATE.IDLE);
        }
    }

    void EnemyAttack()
    {
        // 사망시엔 막기
        if (unitState.state == UNITSTATE.DIE) return;

        AttackTrigger?.Invoke();
        unitState.SetUnitState(UNITSTATE.ATTACK);
        //spawnHitBox();
        lastAttackTime = Time.time;
    }
    // 처음 시간 초기화 세팅
    public void AttackTimeInit() 
    {
        lastAttackTime = Time.time;
    }
    // 공격모션 끝나고 실행할 함수
    public void spawnHitBox()
    {
        float attackPositionX = transform.position.x + (unitState.direction == DIRECTION.RIGHT ? attckDistance : -attckDistance);
        Vector2 spawnPosition = new Vector2(attackPositionX, transform.position.y);
        HitBox spawn = Instantiate(hitBox, spawnPosition, Quaternion.identity);

        spawn.InitializeHitBox(
            unitStat.colliderVerticalOffset,
            unitStat.colliderHorizontalOffset,
            unitStat.statData.atk,
            unitMove2D.moveVector,
            gameObject
        );
    }

}
