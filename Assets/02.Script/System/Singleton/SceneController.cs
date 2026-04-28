using System.IO;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [Header("Stage 목록 설정")]
    [SerializeField] string[] stageCatalog;
    // 현재 씬 종류 출력 임시로 로비 설정
    public static string currentScene;

    private void Awake()
    {
        Instance = this;    

        if (stageCatalog.Length == 0)
        {
            Debug.Log("[SceneController] stageCatalog 설정 안되어있음");
            return;
        }
        if (string.IsNullOrWhiteSpace(currentScene)) currentScene = "Title";
    }    

    public void SceneChange(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName)) return;
        sceneName = sceneName.Trim();

        // 씬컨트롤에선 스테이지 종류이름 받고 다른곳에선 Stage로 받아야함
        currentScene = sceneName;
        if (sceneName == "Stage")
        {
            int random = UnityEngine.Random.Range(0, stageCatalog.Length);
            sceneName = stageCatalog[random];
        }

        if (SceneExists(sceneName)) SceneManager.LoadScene(sceneName);
        
        if(SoundManager.Instance != null) SoundManager.Instance.SfxStop();


    }
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                return true;
            }
        }

        Debug.Log($"[SceneController] sceneName에 해당하는 Scene이 존재하지않습니다. 현재 씬 : {sceneName}");
        return false;
    }

}
