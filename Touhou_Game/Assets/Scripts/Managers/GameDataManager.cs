using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class GameDataManager : MonoBehaviour
{
    public KeyCode saveButton = KeyCode.P;
    public TextMeshProUGUI coinText;

    private GameData gameData;

    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    private void Awake()
    {
        gameData = new GameData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveButton))
        {
            SaveGame();
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

    public void LoadGame()
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
            #if UNITY_WEBGL && !UNITY_EDITOR
            WindowAlert("No saved game data found");
            #else
            Debug.Log("No saved game data found");
            #endif
        }
    }

    public void setUI()
    {
        coinText.text = "Coins: " + gameData.currentCoins.ToString();
    }

    public void addCoins(int coins = 1)
    {
        gameData.totalCoins++;
        gameData.currentCoins++;
        if (gameData.accumulatedCoins <= gameData.currentCoins)
            gameData.accumulatedCoins++;
        
        coinText.text = "Coins: " + gameData.currentCoins.ToString();
    }

    public void removeCoins(int coins = 1)
    {
        gameData.currentCoins -= coins;
    }
}
