using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI[] displayVariables;

    private void Start() {
        GameDataManager gameDataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDataManager>();

        gameDataManager.SetUI(displayVariables);
        StartCoroutine(gameDataManager.CountPlayTime());
    }
}