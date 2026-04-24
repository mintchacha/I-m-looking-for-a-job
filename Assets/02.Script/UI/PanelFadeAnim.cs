using UnityEngine;
using UnityEngine.UI;

public class PanelFadeAnim : MonoBehaviour
{
    private Image panel;
    [SerializeField] private float duration;

    private float timer;
    private bool isAnimating;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    public int blinkCount;

    private void Awake()
    {
        panel = gameObject.GetComponent<Image>();
        if (panel == null) 
        {
            Debug.Log($"[PanelFadeAnim] Image 참조 누락");
            return;
        }
    }
    private void OnEnable()
    {
        panel.color = startColor;
        AnimSet();
    }

    private void Update()
    {
        Animation();
    }

    void Animation()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        t = Mathf.SmoothStep(0f, 1f, t);

        panel.color = Color.Lerp(startColor, endColor, t);

        if (t >= 1f)
        {
            isAnimating = false;
            blinkCount--;
        }
        
    }

    public void AnimSet()
    {
        timer = 0f;
        isAnimating = true;
        if (blinkCount == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Invoke("AnimSet", duration);
        }
    }
}