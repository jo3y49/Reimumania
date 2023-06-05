using UnityEngine;

public class CoinCollector : MonoBehaviour {
    private PlayerData playerData;

    private void Start() {
        playerData = GetComponentInParent<PlayerData>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            playerData.CollectCoin(other.gameObject);
        }
    }
}