using UnityEngine;
using System.Collections;
using TMPro;


public class UIManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI[] displayVariables;
    private GameDataManager gameDataManager;

    private void Start() {
        gameDataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDataManager>();
        gameDataManager.coinText = displayVariables[0];
        gameDataManager.playtimeText = displayVariables[1];

        gameDataManager.setUI();
        StartCoroutine(gameDataManager.countPlayTime());
    }
}