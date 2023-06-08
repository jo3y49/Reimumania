using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour {
    public string currentScene;
    private GameDataManager gameDataManager;
    private PersistenceManager persistenceManager;

    private void Start() {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameDataManager = gameManager.GetComponent<GameDataManager>();
        persistenceManager = gameManager.GetComponent<PersistenceManager>();
    }
    public void Teleport(Vector2 location)
    {
        gameDataManager.SaveLastLocation(currentScene, location);
        persistenceManager.Reset();
        SceneManager.LoadScene("Shrine");
    }
}