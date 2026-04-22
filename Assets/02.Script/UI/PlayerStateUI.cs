using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{


    [SerializeField ] UnitHealth playerHealth;
    [SerializeField] Slider playerHealthSlider;
    float Slidervalue;

    private void Awake()
    {
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
    }

    private void Update()
    {
        playerHealthSlider.value = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
