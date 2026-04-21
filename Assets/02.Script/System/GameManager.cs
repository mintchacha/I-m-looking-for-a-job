using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GAMESTATE gameState { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private void Start()
    {
        gameState = GAMESTATE.PLAYING;
    }
    private void Update()
    {
    }

    public void SetGameState(GAMESTATE gameState)
    {
        this.gameState = gameState;
    }

}
