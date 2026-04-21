using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
            GameObject newEnemy = EnemyQueue.Dequeue();            
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            GameObject enemy = Instantiate(newEnemy, spawnPosition, Quaternion.identity);
            enemy.GetComponentInChildren<UnitChase>().Initialize(target);

            Debug.Log(this + " : " + EnemyQueue.Count + "몬스터존재" );
            Invoke("EnemySpawn", spawnerTime);
        }
        else 
        {
            //Destroy(gameObject);
        }


    }
}
