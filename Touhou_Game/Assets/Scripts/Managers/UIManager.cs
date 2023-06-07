using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI[] displayVariables;
    private GameDataManager gameDataManager;
    private PersistenceManager persistenceManager;
    public GameObject pauseUI;
    private bool isPaused = false;

    private void Start() {
        gameDataManager = GameObject.FindObjectOfType<GameDataManager>();
        persistenceManager = GameObject.FindObjectOfType<PersistenceManager>();

        gameDataManager.SetUI(displayVariables);
        StartCoroutine(gameDataManager.CountPlayTime());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseUI.SetActive(false);
        Time.timeScale = 1f; // This makes everything in the game move at normal speed
        TogglePause(false);
    }

    void Pause() {
        pauseUI.SetActive(true);
        Time.timeScale = 0f; // This makes everything in the game stop moving
        TogglePause(true);
    }

    public void LoadMenu() {
        Time.timeScale = 1f; // You need to make sure that the game isn't still paused when you load another scene
        persistenceManager.Reset();
        SceneManager.LoadScene("TitleScreen");
    }

    private void TogglePause(bool pause)
    {
        isPaused = gameDataManager.isPaused = pause;
    }
}