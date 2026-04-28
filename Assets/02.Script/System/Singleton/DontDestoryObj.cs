using UnityEngine;

public class DontDestoryObj : MonoBehaviour
{
    DontDestoryObj Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)        
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
