using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using TMPro;

public class GameDataManager : MonoBehaviour
{
    public KeyCode saveButton = KeyCode.P;
    public KeyCode deleteButton = KeyCode.O;
    public TextMeshProUGUI livesText, bombText, coinText, killText, playtimeText;
    public bool isPaused = true;

    private GameData gameData;
    private PlayerData player;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveButton))
        {
            SaveGame();
        } else if (Input.GetKeyDown(deleteButton))
        {
            DeleteData();
        }
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

    public void SetUI(TextMeshProUGUI[] displayVariables)
    {
        livesText = displayVariables[0];
        bombText = displayVariables[1];
        coinText = displayVariables[2];
        killText = displayVariables[3];
        playtimeText = displayVariables[4];

        livesText.text = "Lives: " + player.lives.ToString();
        bombText.text = "Bombs: " + player.bombs.ToString();
        coinText.text = "Coins: " + gameData.currentCoins.ToString();
        killText.text = "Kills: " + gameData.kills.ToString();
        playtimeText.text = "Play Time: " + FormatTimeToString(gameData.playTime);
        
        isPaused = false;
    }

    public void GetSavedPlayerData(PlayerData player)
    {
        player.coins = gameData.currentCoins;
        player.upgrade = gameData.spellCardUpgrade;
        this.player = player;
    }

    public void AddCoins(int coins = 1)
    {
        gameData.totalCoins += coins;
        gameData.currentCoins += coins;
        if (gameData.accumulatedCoins < gameData.currentCoins)
            gameData.accumulatedCoins = gameData.currentCoins;
        
        coinText.text = "Coins: " + gameData.currentCoins.ToString();
    }

    public void RemoveCoins(int coins = 1)
    {
        gameData.currentCoins -= coins;

        coinText.text = "Coins: " + gameData.currentCoins.ToString();
    }

    public void SetUpgrade(PlayerData.Upgrade upgrade)
    {
        gameData.spellCardUpgrade = upgrade;
    }

    public void AddKill(int kills = 1)
    {
        gameData.kills += kills;

        killText.text = "Kills: " + gameData.kills.ToString();
    }

    public void setLives(int lives)
    {
        livesText.text = "Lives: " + player.lives.ToString();
    }

    public void SetBombs(int bombs)
    {
        bombText.text = "Bombs: " + player.bombs.ToString();
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
