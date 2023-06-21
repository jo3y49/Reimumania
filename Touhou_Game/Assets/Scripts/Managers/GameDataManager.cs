using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public TextMeshProUGUI livesText, bombText, coinText, killText, playtimeText;
    public HealthDisplay healthBar;
    public BombDisplay bombBar;
    public bool isPaused = true;
    public bool updateUI = false;

    private GameData gameData;
    private PlayerData player;
    private AudioController audioController;
    public string lastScene = "";
    
    public Vector3 lastLocation = Vector3.zero;

    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);


    private void Awake()
    {
        gameData = new GameData();
        audioController = GetComponent<AudioController>();
    }

    public void SaveGame()
    {
        // Convert your data to a string format (e.g., JSON)
        string gameDataString = JsonUtility.ToJson(gameData);

        #if UNITY_WEBGL && !UNITY_EDITOR
        SaveToLocalStorage("GameData", gameDataString);
        SyncFiles(); // Make sure data is written immediately
        #else
        // Use Unity's built-in PlayerPrefs for saving in the editor or standalone build
        PlayerPrefs.SetString("GameData", gameDataString);
        #endif
    }

    public GameData LoadGame()
    {
        string gameDataString;

        #if UNITY_WEBGL && !UNITY_EDITOR
        gameDataString = LoadFromLocalStorage("GameData");
        #else
        gameDataString = PlayerPrefs.GetString("GameData");
        #endif

        // Make sure data was found before trying to parse it
        if (!string.IsNullOrEmpty(gameDataString))
        {
            gameData = JsonUtility.FromJson<GameData>(gameDataString);
        }
        else
        {
            return null;
        }

        return gameData;
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void DeleteData()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        RemoveFromLocalStorage("GameData");
        #else
        PlayerPrefs.SetString("GameData", null);
        #endif
    }

    public void SetUI(TextMeshProUGUI[] displayVariables, HealthDisplay healthBar, BombDisplay bombBar)
    {
        player = FindAnyObjectByType<PlayerData>();

        updateUI = true;

        coinText = displayVariables[0];
        killText = displayVariables[1];
        playtimeText = displayVariables[2];

        coinText.text = gameData.currentCoins.ToString();
        killText.text = "Kills: " + gameData.kills.ToString();
        playtimeText.text = "Play Time: " + FormatTimeToString(gameData.playTime);

        this.healthBar = healthBar;
        this.bombBar = bombBar;
        this.bombBar.ChangeBombs(gameData.bombs);
        this.healthBar.ChangeHearts(gameData.lives);
        
        isPaused = false;
    }

    public void GameOver()
    {
        AddLives(2);
        SceneManager.LoadScene("MainArea");
    }

    public void ReturnToTitle()
    {
        updateUI = false;
        audioController.UnpauseAudio();
        SceneManager.LoadScene("TitleScreen");
    }

    public void GetSavedPlayerData(PlayerData player)
    {
        player.coins = gameData.currentCoins;
        player.lives = gameData.lives;
        player.bombs = gameData.bombs;
        player.upgrade = gameData.spellCardUpgrade;
    }

    // void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // void OnDisable()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "MainArea") 
    //     {
    //         GameObject player = GameObject.FindGameObjectWithTag("Player");
    //         player.transform.position = new Vector2(gameData.lastLocation[0], gameData.lastLocation[1]);
    //     }
    // }

    public void AddCoins(int coins = 1)
    {
        gameData.totalCoins += coins;
        gameData.currentCoins += coins;
        if (gameData.accumulatedCoins < gameData.currentCoins)
            gameData.accumulatedCoins = gameData.currentCoins;

        if (updateUI)
            coinText.text = gameData.currentCoins.ToString();
    }

    public void RemoveCoins(int coins = 1)
    {
        gameData.currentCoins -= coins;
        gameData.spentCoins += coins;

        if (updateUI)
            coinText.text = gameData.currentCoins.ToString();
    }

    public int GetCoins()
    {
        return gameData.currentCoins;
    }

    public void AddLives(int lives = 1)
    {
        gameData.lives += lives;

        if (updateUI)
            healthBar.ChangeHearts(gameData.lives);
    }

    public void LoseLives(int lives = 1)
    {
        gameData.lives -= lives;

        if (updateUI)
            healthBar.ChangeHearts(gameData.lives);
    }

    public int GetLives()
    {
        return gameData.lives;
    }

    public void AddBombs(int bombs = 1)
    {
        gameData.bombs += bombs;

        if (updateUI)
            bombBar.ChangeBombs(gameData.bombs);
    }

    public void LoseBombs(int bombs = 1)
    {
        gameData.bombs -= bombs;

        if (updateUI)
            bombBar.ChangeBombs(gameData.bombs);
    }

    public int GetBombs()
    {
        return gameData.bombs;
    }

    public void SetUpgrade(PlayerData.Upgrade upgrade)
    {
        gameData.spellCardUpgrade = upgrade;
    }

    public PlayerData.Upgrade GetUpgrade()
    {
        return gameData.spellCardUpgrade;
    }

    public void AddKill(int kills = 1)
    {
        gameData.kills += kills;

        killText.text = "Kills: " + gameData.kills.ToString();
    }

    public void SetLastLocation(Vector2 location)
    {
        gameData.lastLocation[0] = lastLocation.x;
        gameData.lastLocation[1] = lastLocation.y;
    }

    public void Pause(bool pause)
    {
        isPaused = pause;

        if (pause)
            audioController.PauseAudio();
        else
            audioController.UnpauseAudio();
    }

    public IEnumerator CountPlayTime()
    {
        float previousTime = Time.time;

        while (true)
        {
            if (!isPaused)
            {
                float deltaTime = Time.time - previousTime;
                gameData.playTime += deltaTime;
            }

            previousTime = Time.time;
            
            playtimeText.text = "Play Time: " + FormatTimeToString(gameData.playTime);

            yield return null;
        }
    }
    private string FormatTimeToString(float time)
    {
        int hours = (int)time / 3600;
        int minutes = ((int)time % 3600) / 60;
        int seconds = ((int)time % 3600) % 60;

        return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
