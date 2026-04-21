using UnityEngine;

public class SystemUI : MonoBehaviour
{
    public static SystemUI Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) return;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
