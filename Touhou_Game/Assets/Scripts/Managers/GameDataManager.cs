using System.Runtime.InteropServices;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
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

    public void SaveGame()
    {
        // Convert your data to a string format (e.g., JSON)
        string gameDataString = JsonUtility.ToJson(gameData);

        SaveToLocalStorage("GameData", gameDataString);
        SyncFiles(); // Make sure data is written immediately
    }

    public void LoadGame()
    {
        string gameDataString = LoadFromLocalStorage("GameData");

        // Make sure data was found before trying to parse it
        if (!string.IsNullOrEmpty(gameDataString))
        {
            gameData = JsonUtility.FromJson<GameData>(gameDataString);
        }
        else
        {
            WindowAlert("No saved game data found");
        }
    }

    public void addCoins(int coins = 1)
    {
        gameData.totalCoins++;
        gameData.currentCoins++;
        if (gameData.accumulatedCoins <= gameData.currentCoins)
            gameData.accumulatedCoins++;
    }

    public void removeCoins(int coins = 1)
    {
        gameData.currentCoins -= coins;
    }
}
