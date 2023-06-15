using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI[] displayVariables;
    [SerializeField] private HealthDisplay healthBar;
    [SerializeField] private BombDisplay bombBar;

    private void Start() {
        GameDataManager gameDataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDataManager>();

        gameDataManager.SetUI(displayVariables, healthBar, bombBar);
        StartCoroutine(gameDataManager.CountPlayTime());
    }
}