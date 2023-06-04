using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class GameDataManager : MonoBehaviour
{
    public KeyCode saveButton = KeyCode.P;
    public KeyCode deleteButton = KeyCode.O;
    public TextMeshProUGUI coinText, playtimeText, killText, deathText;
    public bool isPaused = true;
    public int deaths = 0;

    private GameData gameData;

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
        // if (Input.GetKeyDown(saveButton))
        // {
        //     SaveGame();
        // } else if (Input.GetKeyDown(deleteButton))
        // {
        //     DeleteData();
        // }
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
        coinText = displayVariables[0];
        playtimeText = displayVariables[1];
        killText = displayVariables[2];
        deathText = displayVariables[3];

        coinText.text = "Coins: " + gameData.currentCoins.ToString();
        playtimeText.text = "Play Time: " + FormatTimeToString(gameData.playTime);
        killText.text = "Kills: " + gameData.kills.ToString();
        deathText.text = "Deaths: " + deaths.ToString();

        isPaused = false;
    }

    public void GetSavedPlayerData(PlayerData player)
    {
        player.coins = gameData.currentCoins;
        player.upgrade = gameData.spellCardUpgrade;
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

    public void Death(int death = 1)
    {
        deaths += death;

        deathText.text = "Deaths: " + deaths.ToString();
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
