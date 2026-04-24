using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
// 기본 공격 매니저.
public class BattleManager : MonoBehaviour
{
    [Header("플레이어 상태 컴포넌트")]
    [SerializeField] PlayerInputReader playerinput;
    [SerializeField] PlayerStat playerStat;
    [SerializeField] UnitState playerState;
    [Header("히트박스 종류")]
    GameObject owner;
    // 공격키 입력횟수 콤보  시스템 구현을 위한 큐
    static Queue<int> commandQueue = new Queue<int>();
    static float lastAttackTime = -9999f;

    float inputAttackKeyTime = 0.5f; // 공격키 입력 허용 시간

    public static bool isAttackParam;
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

        // 공격 관련 상태일때 true 
        if (playerState.state == UNITSTATE.JUMPATTACK || playerState.state == UNITSTATE.ATTACK)
        {
            isAttackParam = true;
        }
        else 
        {
            isAttackParam = false;
        }


        // 점프상태일때나 점프공격안했을때 실행
        if (playerState.state == UNITSTATE.JUMP && playerState.state != UNITSTATE.JUMPATTACK)
        {
            JumpAttackManager();
            return;
        }
        else if (playerState.state != UNITSTATE.JUMP && playerState.state != UNITSTATE.JUMPATTACK)
        {
            // 점프나 점프공격 아닐때 실행
            DefaultAttackManager(playerState.state == UNITSTATE.ATTACK);
        }
        // 공격상태에서 시간경과하면 상태 돌리기 (공격상태에서만 작동해야함 아니면 무한 덮어씌워짐)  
        if (playerState.state == UNITSTATE.ATTACK && Time.time > lastAttackTime + inputAttackKeyTime)
        {
            playerState.SetUnitState(UNITSTATE.IDLE);
        }
    }

    void JumpAttackManager()
    {
        if (playerinput.attackPressed)
        {
            playerinput.ResetAttack();
            playerState.SetUnitState(UNITSTATE.JUMPATTACK);
            spawnJumpHitBox();
        }
    }

    void DefaultAttackManager(bool isAttack)
    {
        if (playerState.state != UNITSTATE.DIE && playerinput.attackPressed)
        {
            // 첫공격아닐때 허용 시간안에(0.5) 연속으로 입력하면 카운트 증가
            if (!fullCombo && isAttack) attackCount++;

            // 풀콤보가 true이면 queue가 초기화 되기때문에 미리입력 더 안되게 추가 Enqueue막아야됨
            // 공격시간경과하면 풀콤보 ture에 첫 공격이기 때문에  조건 걸어줌
            if (fullCombo && !isAttack)
            {
                commandQueue.Enqueue(attackCount);
            }
            else if(!fullCombo) // 풀콤보 아니면 카운트 증가한게 들어간다
            {
                commandQueue.Enqueue(attackCount);
            }
            playerState.SetUnitState(UNITSTATE.ATTACK);

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
    public static void SetAttackReset()
    {
        commandQueue.Clear();
        attackCount = 1;
    }

    void spawnHitBox()
    {
        float attackPositionX = transform.position.x + (playerState.direction == DIRECTION.RIGHT ? playerStat.defaultAttack.attckDistance : - playerStat.defaultAttack.attckDistance);
        Vector2 spawnPosition = new Vector2(attackPositionX, transform.position.y);
        HitBox spawn = Instantiate(playerStat.defaultAttack.hitBox, spawnPosition, Quaternion.identity);

        spawn.InitializeHitBox(
            playerStat.defaultAttack.colliderVerticalOffset,
            playerStat.defaultAttack.colliderHorizontalOffset,
            playerStat.statData.atk * playerStat.defaultAttack.damageMultiplier,
            playerinput.MoveVector,
            gameObject
        );        
    }
    void spawnJumpHitBox()
    {
        float attackPositionX = transform.position.x + (playerState.direction == DIRECTION.RIGHT ? playerStat.defaultAttack.JumpVerticalAttckDistance : -playerStat.defaultAttack.JumpVerticalAttckDistance);
        Vector2 spawnPosition = new Vector2(attackPositionX, transform.position.y);
        FollowHitBox spawn = Instantiate(playerStat.defaultAttack.JumpHitBox, spawnPosition, Quaternion.identity);

        spawn.InitializeHitBox(
            playerStat.defaultAttack.JumpColliderVerticalOffset,
            playerStat.defaultAttack.JumpColliderHorizontalOffset,
            playerStat.statData.atk * playerStat.defaultAttack.damageMultiplier,
            playerStat.defaultAttack.JumpVerticalAttckDistance,
            playerStat.defaultAttack.JumpHorizontalAttckDistance,
            playerState,
            gameObject
        );        
    }
}
