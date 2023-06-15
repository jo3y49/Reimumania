using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour {
    public string nextScene;
    private GameDataManager gameDataManager;

    private void Start() {
        gameDataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDataManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            gameDataManager.SetLastLocation(other.transform.position);
            gameDataManager.SaveGame();
            SceneManager.LoadScene(nextScene);
        }
    }
}