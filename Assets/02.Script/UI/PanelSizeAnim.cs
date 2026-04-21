using Unity.VisualScripting;
using UnityEngine;

public class PanelSizeAnim : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private float duration = 0.25f;

    private float timer;
    private bool isOpening;
    private bool isAnimating;
    private Vector3 startScale;
    private Vector3 endScale;

    private void Awake()
    {
        panel = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        panel.localScale = new Vector3(1f, 0f, 1f);
    }

    private void Update()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        t = Mathf.SmoothStep(0f, 1f, t);

        panel.localScale = Vector3.Lerp(startScale, endScale, t);

        if (t >= 1f)
        {
            isAnimating = false;
        }
    }

    public void Open()
    {
        startScale = panel.localScale;
        endScale = new Vector3(1f, 1f, 1f);
        timer = 0f;
        isAnimating = true;

        Invoke("Close", 3f);
    }

    public void Close()
    {
        startScale = panel.localScale;
        endScale = new Vector3(1f, 0f, 1f);
        timer = 0f;
        isAnimating = true;

        Invoke("PanelClose", duration);
    }
    void PanelClose() => gameObject.SetActive(false);

}
