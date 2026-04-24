using UnityEngine;
using UnityEngine.UI;

public class ButtonBinder : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneController.Instance.SceneChange("Stage"));
    }
}
