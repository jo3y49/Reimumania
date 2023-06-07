using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
    public KeyCode pauseKey = KeyCode.P;
    public Button saveButton, homeButton, quitButton;
    private GameDataManager gameDataManager;
    private PersistenceManager persistenceManager;
    public UIManager ui;
    public GameObject pauseUI;

    private bool isPaused = false;

    private void Start() {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameDataManager = gameManager.GetComponent<GameDataManager>();
        persistenceManager = gameManager.GetComponent<PersistenceManager>();

        saveButton.onClick.AddListener(gameDataManager.SaveGame);
        homeButton.onClick.AddListener(Home);
        quitButton.onClick.AddListener(Quit);

        pauseUI.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(pauseKey)) {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void Resume() {
        Time.timeScale = 1f; // This makes everything in the game move at normal speed
        TogglePause(false);
    }

    private void Pause() {
        Time.timeScale = 0f; // This makes everything in the game stop moving
        TogglePause(true);
    }

    private void Home()
    {
        Time.timeScale = 1f; // You need to make sure that the game isn't still paused when you load another scene
        persistenceManager.Reset();
        SceneManager.LoadScene("Shrine");
    }

    private void Quit() {
        Time.timeScale = 1f; // You need to make sure that the game isn't still paused when you load another scene
        persistenceManager.Reset();
        SceneManager.LoadScene("TitleScreen");
    }

    private void TogglePause(bool pause)
    {
        isPaused = gameDataManager.isPaused = pause;
        pauseUI.SetActive(isPaused);
    }
}