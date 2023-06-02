using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button startGameButton;
    public Button loadGameButton;

    private GameDataManager gameDataManager;

    void Start()
    {
        gameDataManager = GetComponent<GameDataManager>();

        // Add click listeners to the buttons
        startGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGame);

        // Check if save data exists
        if (!PlayerPrefs.HasKey("GameData"))
        {
            // If save data does not exist, disable the Load Game button
            loadGameButton.interactable = false;
        }
    }

    void StartNewGame()
    {
        // Load the scene for a new game
        SceneManager.LoadScene("Main"); // Replace with your game scene name
    }

    void LoadGame()
    {
        // Load the scene for the existing game
        gameDataManager.LoadGame();

        SceneManager.LoadScene("Main"); // Replace with your game scene name
    }
}
