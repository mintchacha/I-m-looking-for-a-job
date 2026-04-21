using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[RequireComponent(typeof(Animator))]
public class StageScore : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI scoreUI;

    string rankName = "Srank";
    int rankNameHash;

    Animator rankAnim;
    private void Awake()
    {
        if (image == null)
        {
            Debug.Log($"[StageScore] image 연결 안됨.");
            return;
        }
        rankAnim = image.GetComponent<Animator>();
        if (rankAnim == null)
        {
            Debug.Log($"[StageScore] rankAnim 연결 안됨.");
            return;
        }

        rankAnim.enabled = false; 
        rankNameHash = Animator.StringToHash(rankName);

    }


    private void Update()
    {
        image.sprite = RewardManager.Instance.result;
        scoreUI.text = "남은 적 수 : " + WaveManager.Instance.currentStayEnemy;
        // 임시 애니메이션세팅\
        if (RewardManager.Instance.nowRank == "S") {
            rankAnim.enabled = true;
            rankAnim.SetBool(rankNameHash, true);
        }
        
    }
}
