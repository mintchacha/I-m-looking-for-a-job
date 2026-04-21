using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }
    [SerializeField] RewardUI rewardUI;
    [Header("플레이어 상태 참조")]
    [SerializeField] UnitState playerState;

    [Header("배틀 스코어 달성조건 B - A - S순")]
    [SerializeField] int[] rankScore;
    string[] rank = new string[] { "C", "B", "A", "S" };
    [Header("랭크 스프라이트 세팅")]
    public Sprite[] rankSprite;

    // 한번만실행되게 하기위한 코드
    bool DeadTrigger = false;

    public string nowRank { get; private set; }
    public int score { get; private set; }
    public Sprite result { get; private set; }

    private void Awake()
    {
        // 다른 Scene엔 필요없을거같아서 지움
        //if (Instance != null && Instance != this) 
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        if (rewardUI == null)
        {
            Debug.Log($"[RewardManager] RewardUI 연결 안됨. {rewardUI}");
            return;
        }
        if (playerState == null)
        {
            Debug.Log($"[RewardManager] UnitState 연결 안됨. {playerState}");
            return;
        }
    }

    private void Start()
    {
        score = 0;
    }

    private void Update()
    {
        ScoreAsRank();

        if (playerState.state == UNITSTATE.DIE && !DeadTrigger)
        {
            DeadTrigger = true;
            Invoke("SpawnRewardUI", 1f);
        }
    }

    public void IncreseScore(int amount)
    {
        if (amount <= 0) return;
        score += amount;
    }

    // 랭크 계산
    public void ScoreAsRank()
    {
        for (int i = 0; i < rankScore.Length; i++)
        {
            if (score < rankScore[i])
            {
                result = rankSprite[i];
                nowRank = rank[i];
                return;
            }
            else 
            {
                result = rankSprite[rankSprite.Length-1];
                nowRank = rank[rank.Length-1];
            }
        }
    }

    public void SpawnRewardUI()
    {
        SoundManager.Instance.BgmStop();
        SoundManager.Instance.SfxPlay("StageEnd");
        rewardUI.gameObject.SetActive(true);
    }
}
