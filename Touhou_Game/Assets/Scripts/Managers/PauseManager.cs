using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
    public Button saveButton, quitButton, deleteButton;
    public GameDataManager gameDataManager;
    public UIManager ui;

    private void Start() {
        gameDataManager = GameObject.FindObjectOfType<GameDataManager>();

        saveButton.onClick.AddListener(gameDataManager.SaveGame);
        quitButton.onClick.AddListener(ui.LoadMenu);
        deleteButton.onClick.AddListener(gameDataManager.DeleteData);
        deleteButton.onClick.AddListener(ui.LoadMenu);
    }
}