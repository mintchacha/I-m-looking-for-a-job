using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpecialAttack : MonoBehaviour
{
    public Queue<GameObject> HitEnemyQueue = new Queue<GameObject>();

    public int id;
    public float damage;
    public float duration = 4f;
    public float needEnerge;

    [Header("히트박스 종류")]
    public HitBoxCircle hitBox;
    [Header("피격 범위 설정")]
    public float radiusOffset;

    // 테스트용 
    int count = 0;

    private void Awake()
    {
        if (hitBox == null)
        {
            Debug.Log("[BattleManager] hitBox가 할당되지 않음");
            return;
        }        
    }
    public void Hit(GameObject enemy)
    {
        //HitEnemyQueue.Enqueue(enemy);
        if (!HitEnemyQueue.Contains(enemy)) HitEnemyQueue.Enqueue(enemy);

        UnitHealth targetHealth = enemy.gameObject.GetComponent<UnitHealth>();
        UnitAnim targetAnim = enemy.gameObject.GetComponentInChildren<UnitAnim>();
        UnitState targetState = enemy.gameObject.GetComponent<UnitState>();
        if (targetHealth == null || targetAnim == null || targetState == null)
        {
            Debug.Log($"[SpecialAttack] {enemy.name}대상에 targetMove혹은 targetAnim 혹은 targetState 가 없습니다");
            return;
        }
        Debug.Log(HitEnemyQueue.Count + "명 맞음");
        targetState.SetUnitState(UNITSTATE.DAMAGED);
        targetState.spcialState = true;
        targetAnim.OnStop();
        Invoke("EffectEnd", duration);

    }
    public void EffectEnd()
    {        
        GameObject enemy = HitEnemyQueue.Dequeue();

        Debug.Log(enemy + "(해제), 남은 해제 적" + HitEnemyQueue.Count);

        if (enemy == null) return;

        UnitHealth targetHealth = enemy.gameObject.GetComponent<UnitHealth>();
        UnitAnim targetAnim = enemy.gameObject.GetComponentInChildren<UnitAnim>();
        UnitState targetState = enemy.gameObject.GetComponent<UnitState>();

        targetAnim.OnPlay();
        targetState.spcialState = false;
        // 지속시간 경과시 체력0이면 죽음
        if (targetHealth.currentHealth == 0)
        {
            targetState.SetUnitState(UNITSTATE.DIE);
        }
        else
        {
            targetState.SetUnitState(UNITSTATE.IDLE);
        }
    }
}