using System.Collections.Generic;
using UnityEngine;

// WaveManager 에서 받아온 리스트 Queue에 담아서 스폰시키는용도
public class EnemySpawner : MonoBehaviour
{
    public Queue<GameObject> EnemyQueue = new Queue<GameObject>();
    [Header("스폰 시간")]
    public float spawnerTime = 3f;

    [Header("추격 대상 설정")]
    [SerializeField] GameObject target;
    private void Awake()
    {
        if (target == null)
        {
            Debug.Log("[EnemySpawner] 추적target 설정 안되어있음");
            return;
        }
    }

    public void EnemySpawn()
    {
        // 스폰 시작시간으로부터 라운드 
        if (EnemyQueue.Count > 0)
        {
            //Debug.Log(gameObject + "| 남은 대기열 수 : " + EnemyQueue.Count);
            GameObject newEnemy = EnemyQueue.Dequeue();            
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            GameObject enemy = Instantiate(newEnemy, spawnPosition, Quaternion.identity);
            UnitChase enemyChase = enemy.GetComponentInChildren<UnitChase>();
            if (enemyChase == null) 
            {
                Debug.Log("[EnemySpawner] enemyChase를 찾을 수 없습니다.");
                return;
            }
            enemyChase.Initialize(target);

            //스폰된 적 정보
            //UnitStat enemyStat = newEnemy.GetComponent<UnitStat>();
            //if (enemyStat == null) Debug.Log(enemy + "에 UnitStat를 찾을 수 없습니다.");
            //if (enemyStat.name == "EliteEnemy") Debug.Log("엘리트 몬스터 등장!");
            //else { Debug.Log(enemyStat.name + "스폰"); }

            Invoke("EnemySpawn", spawnerTime);
        }
        else 
        {
            //Debug.Log("스폰 종료 대기열 : " + EnemyQueue.Count);
        }
        //Invoke("EnemySpawn", spawnerTime);

    }
}
