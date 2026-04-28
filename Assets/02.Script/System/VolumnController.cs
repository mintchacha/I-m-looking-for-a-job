using UnityEngine;
using UnityEngine.UI;

public class VolumnController : MonoBehaviour
{
    [Header("오디오 볼륨")]
    [SerializeField] Slider slider;

    private void Awake()
    {
        if (slider == null) 
        {
            Debug.Log("[VolumnController] slider 가 참조되지 않았습니다.");
            return;
        }        
    }
    private void Start()
    {
        slider.value = SoundManager.Instance.bgmSource.volume;
    }
    private void Update()
    {
        SoundManager.Instance.SetBgmSource(slider.value);
    }
}
