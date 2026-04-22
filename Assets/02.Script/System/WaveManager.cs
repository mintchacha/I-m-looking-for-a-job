using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("웨이브 시작 정보")]
    [SerializeField] PanelSizeAnim wavePanel;
    [SerializeField] TextMeshProUGUI waveTitle;
    [SerializeField] TextMeshProUGUI waveCountText;

    // 필요, 몬스터 종류 , 스폰 수 설정
    [Serializable]
    public struct EnemyInfo
    {
        public int spawn;
        public GameObject enemyPrefab;
    }
    [Serializable]
    public struct WaveInfo
    {
        public string waveId;
        public EnemyInfo normalEnemy;
        public EnemyInfo eliteEnemy;
        public EnemyInfo boss;
    }

    [Header("좌측 스포너")]
    [SerializeField] EnemySpawner spawner1;
    [Header("우측 스포너")]
    [SerializeField] EnemySpawner spawner2;
    [Header("웨이브 세팅 (10번마다 엘리트)")]
    [SerializeField] List<WaveInfo> waveList;
    // 실제 동작할 변수
    WaveInfo currentWave;
    // 현재 웨이브
    int waveCount = 0;
    // 존재중인 적 개체수
    public int currentStayEnemy { get; private set; }

    [SerializeField] bool debugMode = false;

    private void Awake()
    {
        Instance = this;

        if (waveList.Count == 0)
        {
            Debug.Log("[WaveManager] waveSetting 설정 안되어있음");
            return;
        }
        if (spawner1 == null || spawner2 == null)
        {
            Debug.Log("[WaveManager] spawner 참조 안되어있음");
            return;
        }
        
        WaveSetting();
    }
    private void Update()
    {
        if (debugMode) Debug.Log("필드 내 적의 수 : " + currentStayEnemy);
    }
    void NextWaveSetting()
    {
        waveCount++;
        if (waveCount >= waveList.Count)
        {
            RewardManager.Instance.SpawnRewardUI();
            return;
        }
        Debug.Log(waveCount+1 + "라운드 시작");
        WaveSetting();
    }
    void WaveSetting()
    {
        currentWave = waveList[waveCount];
        // 처음 최대 적 수 세팅한 후 Queue 넘겨주고 적이  죽으면 줄어들것
        currentStayEnemy = currentWave.normalEnemy.spawn + currentWave.eliteEnemy.spawn + currentWave.boss.spawn;

        EnemyQueueInput();

        // 웨이브 시작 ui
        waveTitle.text = currentWave.waveId;
        waveCountText.text = "목표 적 : " + currentStayEnemy;
        wavePanel.gameObject.SetActive(true);
        wavePanel.Open();

    }

    public void EnemyQueueInput()
    {
        spawner1.EnemyQueue.Clear();
        spawner2.EnemyQueue.Clear();

        for (int i = 0; i < currentStayEnemy; i++)
        {
            GameObject enemy;
            if (currentWave.boss.spawn > 0) // 보스는 필드에 1개체만 존재할것.
            {
                enemy = currentWave.boss.enemyPrefab;
                currentWave.boss.spawn--;
            }
            else if (currentWave.eliteEnemy.spawn > 0 && (i+1) % 9 == 0) // 10번째 마다 엘리트 입력 i+1은 0일때도 0%9 가 0 이기때문에
            {
                // 10번째마다 엘리트 출현
                if (currentWave.eliteEnemy.enemyPrefab == null) Debug.Log("엘리트 프리팹이 null 입니다.");
                enemy = currentWave.eliteEnemy.enemyPrefab;
                currentWave.eliteEnemy.spawn--;
            }
            else
            {
                enemy = currentWave.normalEnemy.enemyPrefab;
                currentWave.normalEnemy.spawn--;
            }

            Debug.Log(enemy);

            if (enemy != null)
            {
                // 보스존재하면 우측 스포너에서 출현, 그래서 시작 스폰은 죄측
                if (i % 2 == 0)
                {
                    spawner2.EnemyQueue.Enqueue(enemy);
                }
                else
                {
                    spawner1.EnemyQueue.Enqueue(enemy);
                }                    
            }
        }

        if (debugMode) 
        {
            Debug.Log("Spawner 죄측 대기열" + spawner1.EnemyQueue.Count);
            Debug.Log("Spawner 우측 대기열" + spawner2.EnemyQueue.Count);
        }

        // 8마리까지만나오는.
        spawner1.EnemySpawn();
        spawner2.EnemySpawn();
    }

    public void DecreaseStayEnemy() 
    {
        currentStayEnemy--;
        if (currentStayEnemy == 0) Invoke("NextWaveSetting", 2f);
    }


    

}
