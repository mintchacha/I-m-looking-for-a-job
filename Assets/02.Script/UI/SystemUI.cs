using UnityEngine;

public class SystemUI : MonoBehaviour
{
    public static SystemUI Instance;
    private void Awake()
    {
        Instance = this;
    }
}
