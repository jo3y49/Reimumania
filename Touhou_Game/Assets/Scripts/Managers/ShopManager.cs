using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour {
    public GameObject popup;
    public Button[] buyables;
    public TextMeshProUGUI[] quantity;
    public int[] prices;
    public Button back, popConfirm, popBack;
    public TextMeshProUGUI coins;
    private GameDataManager gameDataManager;
    private Coroutine popupCoroutine;
    private bool confirm = false;

    // buyables: life, bomb, spellcard

    private void Start() {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameDataManager = gameManager.GetComponent<GameDataManager>();

        coins.text = gameDataManager.GetCoins().ToString();

        for (int i = 0; i < buyables.Length; i++)
        {
            buyables[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Price: (" + prices[i] + " coins)";
            int index = i;
            buyables[i].onClick.AddListener(() => Buy(index));
        }
        SetShop();

        popConfirm.onClick.AddListener(Confirmed);
        popBack.onClick.AddListener(Unconfirmed);
        back.onClick.AddListener(EnterMainArea);

        popup.SetActive(false);
    }

    private void SetShop()
    {
        coins.text = gameDataManager.GetCoins().ToString();

        quantity[0].text = "Lives: Max 3         Currently Have " + gameDataManager.GetLives();
        quantity[1].text = "Bombs: Max 3 Currently Have " + gameDataManager.GetBombs();

        for (int i = 0; i < buyables.Length; i++)
        {

            if (prices[i] > gameDataManager.GetCoins() || i == 0 && gameDataManager.GetLives() >= 3 ||
            i == 1 && gameDataManager.GetBombs() >= 3 || i == 2 && gameDataManager.GetUpgrade() == PlayerData.Upgrade.L2)
            {
                buyables[i].interactable = false;
            }
        }
    }

    private void Buy(int index)
    {
        if (popupCoroutine == null)
        {
            popupCoroutine = StartCoroutine(Confirm(index));
        }
            
    }

    private IEnumerator Confirm(int index)
    {
        confirm = true;

        popup.SetActive(true);

        while (confirm)
        {
            yield return null;
        }

        MakePurchase(index);
    }

    private void MakePurchase(int index)
    {
        switch(index)
        {
            case 0:
            BuyLives();
            break;
            case 1:
            BuyBombs();
            break;
            case 2:
            BuyUpgrade();
            break;
        }


        gameDataManager.RemoveCoins(prices[index]);
        SetShop();
    }

    private void BuyLives()
    {
        gameDataManager.AddLives();
    }

    private void BuyBombs()
    {
        gameDataManager.AddBombs();
    }

    private void BuyUpgrade()
    {
        gameDataManager.SetUpgrade(PlayerData.Upgrade.L2);
    }

    private void Confirmed()
    {
        popup.SetActive(false);
        confirm = false;
        popupCoroutine = null;
    }

    private void Unconfirmed()
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
            popupCoroutine = null;
        }

        popup.SetActive(false);
        confirm = false;
    }

    private void EnterMainArea()
    {
        SceneManager.LoadScene("MainArea");
    }
}