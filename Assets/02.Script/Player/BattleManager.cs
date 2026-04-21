using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("플레이어 상태 컴포넌트")]
    [SerializeField] PlayerInputReader playerinput;
    [SerializeField] PlayerStat playerStat;
    [SerializeField] UnitState playerState;
    [Header("히트박스 종류")]
    [SerializeField] HitBox hitBox;
    GameObject owner;
    // 공격키 입력횟수 콤보  시스템 구현을 위한 큐
    static Queue<int> commandQueue = new Queue<int>();
    static float lastAttackTime = -9999f;

    [Header("공격 적용 거리")]
    public float attckDistance = 0.3f;
    float inputAttackKeyTime = 0.5f; // 공격키 입력 허용 시간

    [Header("공격중 여부")]
    public static bool isAttack;
    bool fullCombo = false;
    // 입력시마다 대기열에 넣을 값
    static int attackCount = 1;
    // 공격속도에 따라 카운트되는 값
    public static int currentAttackCount = 1;

    [SerializeField]bool debugMode = true;

    private void Awake()
    {
        if (playerinput == null)
        {
            Debug.Log("[BattleManager] PlayerInputReader가 할당되지 않음");
            return;
        }
        if (playerStat == null)
        {
            Debug.Log("[BattleManager] PlayerStat이 할당되지 않음");
            return;
        }
        if (hitBox == null)
        {
            Debug.Log("[BattleManager] hitBox가 할당되지 않음");
            return;
        }
        if (playerState == null)
        {
            Debug.Log("[BattleManager] UnitState가 할당되지 않음");
            return;
        }
    }

    private void Update()
    {
        // 사망중 상태면 작동x
        if (playerState.state == UNITSTATE.DIE) return;
        //공격중인지 여부는 마지막 공격 후, 키입력 허용시간 동안 지속
        if (Time.time < lastAttackTime + inputAttackKeyTime)
        {
            playerState.SetUnitState(UNITSTATE.ATTACK);
        } else if (playerState.state == UNITSTATE.ATTACK && Time.time > lastAttackTime + inputAttackKeyTime)
        {
            // 공격상태에서 시간경과하면 상태 돌리기 (공격상태에서만 작동해야함 아니면 무한 덮어씌워짐)            
            playerState.SetUnitState(UNITSTATE.IDLE);
        }
            isAttack = playerState.state == UNITSTATE.ATTACK;
        // 기본공격 매니저 실행 사망 혹은 점프 아닐때
        if (playerState.state != UNITSTATE.JUMP) DefaultAttackManager(isAttack);
    }

    void DefaultAttackManager(bool isAttack)
    {
        if (debugMode) Debug.Log("공격중 : " + isAttack);

        if (playerState.state != UNITSTATE.DIE && playerinput.attackPressed)
        {
            // 공격키 입력 시 공격카운트 증가 콤보 허용 시간안에(0.5) 연속으로 입력하면 카운트 증가
           if (!fullCombo && isAttack) attackCount++;

            // 마지막공격했으면 키입력시간 경과되었을때부터 배열입력받기, 마지막 공격 아니면 그냥
            if (fullCombo && !isAttack)
            {
                commandQueue.Enqueue(attackCount);
            }
            else if(!fullCombo)
            {
                commandQueue.Enqueue(attackCount);
            }

                playerinput.ResetAttack();
            if (debugMode) string.Join(",", commandQueue);
        }


        // 대기열에 키입력존재하고 공격속도 시간 지났으면 공격실행
        if (commandQueue.Count > 0 && Time.time > lastAttackTime + playerStat.statData.atkSpeed)
        {
            // BUT 마지막 공격일때 공격중상태면 (0.5초간) 공격실행 막기 그 외엔 false로 풀콤보 해제            
            if (fullCombo && isAttack) return;
            else fullCombo = false;

            ComboAttack();
        }
    
    }

    // 공격과 히트박스생성 혹은 마지막 공격일때 대기열 초기화.
    void ComboAttack()
    {
        currentAttackCount = commandQueue.Dequeue();
        lastAttackTime = Time.time;
        spawnHitBox();

        // 연속기 최대시전시 처음으로 돌아가기
        if (playerStat.defaultAttack.maxComboCount == currentAttackCount)
        {
            fullCombo = true;
            SetAttackReset();
        }               
    }

    void spawnHitBox()
    {
        float attackPositionX = transform.position.x + (playerState.direction == DIRECTION.RIGHT ? attckDistance : -attckDistance);
        Vector2 spawnPosition = new Vector2(attackPositionX, transform.position.y);
        HitBox spawn = Instantiate(hitBox, spawnPosition, Quaternion.identity);

        spawn.InitializeHitBox(
            playerStat.defaultAttack.colliderVerticalOffset,
            playerStat.defaultAttack.colliderHorizontalOffset,
            playerStat.statData.atk * playerStat.defaultAttack.damageMultiplier,
            playerinput.MoveVector,
            gameObject
        );        
    }
    public static void SetAttackReset()
    {
        commandQueue.Clear();
        attackCount = 1;
    }
}
