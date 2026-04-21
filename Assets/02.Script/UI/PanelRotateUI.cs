using UnityEngine;

public class PanelRotateUI : MonoBehaviour
{
    private RectTransform panel;
    public float duration = 0.25f;

    private float timer;
    private bool isOpen = false;
    private bool isAnimating = false;

    Quaternion startRotate;
    Quaternion endRotate;

    [Header("애니메이션 회전 지정")]
    public Vector3 fromRotation;
    public Vector3 toRotate;

    private void Awake()
    {
        panel = GetComponent<RectTransform>();
    }
    private void Start()
    {
        startRotate = Quaternion.Euler(fromRotation);
        endRotate = Quaternion.Euler(toRotate);
    }

    private void Update()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        t = Mathf.SmoothStep(0f, 1f, t);

        panel.localRotation = Quaternion.Lerp(startRotate, endRotate, t);

        if (t >= 1f)
        {
            isAnimating = false;
        }
    }

    public void RotateToggle()
    {
        if (isOpen)
        {
            startRotate = Quaternion.Euler(toRotate);
            endRotate = Quaternion.Euler(fromRotation);
        }
        else
        {
            startRotate = Quaternion.Euler(fromRotation);
            endRotate = Quaternion.Euler(toRotate);
        }
        isAnimating = true;
        isOpen = !isOpen;
        timer = 0f;
    }
}
