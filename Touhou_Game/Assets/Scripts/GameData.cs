[System.Serializable]
public class GameData
{
    public string lastScene = "MainArea";
    public float[] lastLocation = {0,0};
    public int currentCoins = 500;
    public int lives = 1;
    public int bombs = 0;
    public PlayerData.Upgrade spellCardUpgrade = PlayerData.Upgrade.L1;
    public float playTime = 0;
    public int kills = 0;
    public int totalCoins = 0;
    public int spentCoins = 0;
    public int accumulatedCoins = 0;

}