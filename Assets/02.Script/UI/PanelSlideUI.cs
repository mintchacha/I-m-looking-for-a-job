using UnityEngine;

public class PanelSlideUI : MonoBehaviour
{
    private RectTransform panel;
    public float duration = 0.25f;

    private float timer;
    private bool isOpen = false;
    private bool isAnimating = false;
    Vector3 startPosition;
    Vector3 endPosition;
    [Header("애니메이션 좌표 지정")]
    public Vector3 startVector;
    public Vector3 endVector;

    private void Awake()
    {
        panel = GetComponent<RectTransform>();
    }
    private void Start()
    {
        startPosition = startVector;
        endPosition = endVector;
    }

    private void Update()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        t = Mathf.SmoothStep(0f, 1f, t);

        panel.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t >= 1f)
        {
            isAnimating = false;
        }
    }

    public void OpenToggle()
    {
        if (isOpen)
        {
            startPosition = endVector;
            endPosition = startVector;
        }
        else
        {
            startPosition = startVector;
            endPosition = endVector;
        }
        isAnimating = true;
        isOpen = !isOpen;
        timer = 0f;
    }
}
