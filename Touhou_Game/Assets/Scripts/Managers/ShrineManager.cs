using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShrineManager : MonoBehaviour
{
    public Button enterTestArea, enterTutorial, saveGame, quitGame, enterShop, enterBoss;
    public GameObject shopUI;
    private GameDataManager gameDataManager;

    private void Start() {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameDataManager = gameManager.GetComponent<GameDataManager>();

        enterTestArea.onClick.AddListener(EnterTestArea);
        enterTutorial.onClick.AddListener(EnterTutorial);
        saveGame.onClick.AddListener(gameDataManager.SaveGame);
        quitGame.onClick.AddListener(Quit);
        enterShop.onClick.AddListener(EnterShop);
        enterBoss.onClick.AddListener(EnterBoss);
    }
    private void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    private void EnterTestArea()
    {
        SwitchScene("TestArea");
    }
    private void EnterTutorial(){
        SwitchScene("Tutorial");
    }
    private void Quit()
    {
        SwitchScene("TitleScreen");
    }

    private void EnterBoss()
    {
        SwitchScene("Boss");
    }

    private void EnterShop()
    {
        shopUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
