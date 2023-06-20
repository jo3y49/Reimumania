using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
    public KeyCode pauseKey = KeyCode.P;
    public Button saveButton, quitButton;
    private GameDataManager gameDataManager;
    public UIManager ui;
    public GameObject pauseUI;

    private bool isPaused = false;

    private void Start() {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameDataManager = gameManager.GetComponent<GameDataManager>();

        saveButton.onClick.AddListener(gameDataManager.SaveGame);
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

    private void Quit() {
        Time.timeScale = 1f; // You need to make sure that the game isn't still paused when you load another scene
        gameDataManager.ReturnToTitle();
    }

    private void TogglePause(bool pause)
    {
        isPaused = pause;
        gameDataManager.Pause(pause);
        pauseUI.SetActive(isPaused);
    }
}