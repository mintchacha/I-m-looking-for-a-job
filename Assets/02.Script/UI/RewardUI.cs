using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [Header("·©Å· Ãâ·Â")]
    //[SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] Image rankSprite;

    private void Awake()
    {
        if (rankSprite == null)
        {
            Debug.Log($"[RewardUI] Image ¿¬°á ¾ÈµÊ. {rankSprite}");
            return;
        }
    }

    private void OnEnable()
    {
        rankSprite.sprite = RewardManager.Instance.result;        
    }
    public void ChangeScene(string sceneName) => SceneController.Instance.SceneChange(sceneName);
}
