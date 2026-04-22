using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField ] GameObject player;
    [SerializeField] Slider playerHealthSlider;
    [SerializeField] Slider playerSpecialAttackEnergeSlider;
    [SerializeField] Image playerSpecialAttackUI;

    UnitHealth playerHealth;
    float Slidervalue;

    private void Awake()
    {
        playerHealth = player.GetComponent<UnitHealth>();
        if (playerHealth == null)
        {
            Debug.Log("[PlayerStateUI] UnitHealth이 참조 누락");
            return;
        }
        if (playerHealthSlider == null)
        {
            Debug.Log("[PlayerStateUI] Slider 참조 누락");
            return;
        }
        if (playerSpecialAttackUI == null)
        {
            Debug.Log("[PlayerStateUI] playerSpecialAttackUI 참조 누락");
            return;
        }
    }

    private void Update()
    {
        playerHealthSlider.value = playerHealth.currentHealth / playerHealth.maxHealth;
        playerSpecialAttackEnergeSlider.value = PlayerStat.specialAttackEnerge / 100;
        if (PlayerStat.isSpecialAttack)
        {
            playerSpecialAttackUI.color = new Color(1f, 1f, 1f);
        }
        else
        {
            playerSpecialAttackUI.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
