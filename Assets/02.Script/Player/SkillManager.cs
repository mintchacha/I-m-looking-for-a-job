using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
// 기본 공격 매니저.
public class SkillManager : MonoBehaviour
{
    [Header("플레이어 상태 컴포넌트")]
    [SerializeField] PlayerInputReader playerinput;
    [SerializeField] PlayerStat playerStat;
    [SerializeField] UnitState playerState;
    GameObject owner;

    [Header("플레이어와의 거리")]
    public float attckDistance;

    //[SerializeField] bool debugMode = true;

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

        if (playerinput.specialAttackPressed && (playerState.state != UNITSTATE.JUMP)) OnSpecialAttack();

    }


    public void OnSpecialAttack()
    {
        // 키입력 리셋
        playerinput.ResetSpecialAttack();
        // 발동조건 충족하지않으면 막기
        if (!playerStat.isSpecialAttack) return;

        // 애니메이션재생위해 상태변경
        playerState.SetUnitState(UNITSTATE.SPECIALATTACK); 
        PlayerStat.specialAttackEnerge = 0;

        spawnHitBox(playerStat.specialAttack.hitBox);

    }


    void spawnHitBox(HitBoxCircle hitBox)
    {
        float attackPositionX = transform.position.x + (playerState.direction == DIRECTION.RIGHT ? attckDistance : -attckDistance);
        Vector2 spawnPosition = new Vector2(attackPositionX, transform.position.y);
        HitBoxCircle spawn = Instantiate(hitBox, spawnPosition, Quaternion.identity);

        spawn.InitializeHitBox(
            playerStat.specialAttack.radiusOffset,
            playerStat.specialAttack.damage,
            playerinput.MoveVector,
            gameObject
        );        
    }
}
