using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button startGameButton, loadGameButton;

    private GameDataManager gameDataManager;

    void Start()
    {
        gameDataManager = GameObject.FindObjectOfType<GameDataManager>();

        // Add click listeners to the buttons
        startGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGame);

        // Disable the Load Game button by default
        loadGameButton.interactable = false;

        // Try to load the game data
        GameData gameData = gameDataManager.LoadGame();
        
        // If gameData is not null, then save data exists and Load Game button can be enabled
        if (gameData != null)
        {
            loadGameButton.interactable = true;
        }
    }

    private void StartNewGame()
    {
        gameDataManager.NewGame();

        LoadGame();
    }

    private void LoadGame()
    {

        SceneManager.LoadScene("Shrine");
    }
}
