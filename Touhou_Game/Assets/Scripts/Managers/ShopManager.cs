using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour {
    public GameObject shrineUI, popup;
    public Button[] buyables;
    public int[] prices;
    public Button back, popConfirm, popBack;
    public TextMeshProUGUI coins;
    private GameDataManager gameDataManager;
    private Coroutine popupCoroutine;
    private bool confirm = false;

    // buyables: life, bomb, spellcard1, spellcard2

    private void Start() {
        gameDataManager = GameObject.FindObjectOfType<GameDataManager>();

        coins.text = "Coins: " + gameDataManager.GetCoins().ToString();

        for (int i = 0; i < buyables.Length; i++)
        {
            buyables[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += ": (" + prices[i] + " coins)";
            int index = i;
            buyables[i].onClick.AddListener(() => Buy(index));
            if (prices[i] > gameDataManager.GetCoins())
            {
                buyables[i].interactable = false;
            }
        }
        popConfirm.onClick.AddListener(Confirmed);
        popBack.onClick.AddListener(Unconfirmed);
        back.onClick.AddListener(EnterShrine);

        popup.SetActive(false);
        gameObject.SetActive(false);
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
            BuyUpgrade1();
            break;
            case 3:
            BuyUpgrade2();
            break;
        }


        gameDataManager.RemoveCoins(prices[index]);
        coins.text = "Coins: " + gameDataManager.GetCoins().ToString();
    }

    private void BuyLives()
    {
        gameDataManager.AddLives();
    }

    private void BuyBombs()
    {
        gameDataManager.AddBombs();
    }

    private void BuyUpgrade1()
    {
        gameDataManager.SetUpgrade(PlayerData.Upgrade.L2);
    }

    private void BuyUpgrade2()
    {

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

    private void EnterShrine()
    {
        shrineUI.gameObject.SetActive(true);
        Unconfirmed();
        gameObject.SetActive(false);
    }
}