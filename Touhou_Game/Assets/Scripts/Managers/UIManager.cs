using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI[] displayVariables;
    private GameDataManager gameDataManager;

    private void Start() {
        gameDataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDataManager>();
        gameDataManager.coinText = displayVariables[0];

        gameDataManager.setUI();
    }
}