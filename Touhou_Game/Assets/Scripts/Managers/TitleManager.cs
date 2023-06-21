using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button startGameButton, loadGameButton, quitButton, wipeDataButton;

    private GameDataManager gameDataManager;

    void Start()
    {
        gameDataManager = GameObject.FindObjectOfType<GameDataManager>();

        // Add click listeners to the buttons
        startGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGame);
        quitButton.onClick.AddListener(CloseGame);
        wipeDataButton.onClick.AddListener(WipeData);

        // Disable the Load Game button by default
        loadGameButton.interactable = false;
        wipeDataButton.interactable = false;

        // Try to load the game data
        GameData gameData = gameDataManager.LoadGame();
        
        // If gameData is not null, then save data exists and Load Game button can be enabled
        if (gameData != null)
        {
            loadGameButton.interactable = true;
            wipeDataButton.interactable = true;
        }
    }

    private void WipeData()
    {
        gameDataManager.DeleteData();
        loadGameButton.interactable = false;
        wipeDataButton.interactable = false;
    }

    private void LoadGame()
    {

        SceneManager.LoadScene("MainArea");
    }

    private void CloseGame(){
        Application.Quit();
    }

    private void StartNewGame()
    {
        gameDataManager.NewGame();

        LoadGame();
    }
}
