using System;
using UnityEngine;
using UnityEngine.UI;


public class ButtonBinder : MonoBehaviour
{
    public string targetName;

    [SerializeField] GameObject targetObject;

    private void OnEnable()
    {
        if (String.IsNullOrWhiteSpace(targetName))
        {
            Debug.Log("[targetName] targetName 이 비어있습니다.");
            return;
        }

        GetComponent<Button>().onClick.AddListener(() => SceneButton());
    }

    void SceneButton()
    {
        targetName = targetName.Trim();

        if (targetName.Trim() == "Exit")
        {
            QuitGame();
            return;
        }
        if (targetName.Trim() == "Close")
        {
            if (targetObject == null) 
            {
                Debug.Log("[ButtonBinder] 닫을 대상이 없습니다.");
                return;
            }
            targetObject.gameObject.SetActive(false);
            return;
        }
        if (targetName.Trim() == "Open")
        {
            if (targetObject == null) 
            {
                Debug.Log("[ButtonBinder] 열 대상이 없습니다.");
                return;
            }
            targetObject.gameObject.SetActive(true);
            return;
        }

        SceneController.Instance.SceneChange(targetName.Trim());
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
