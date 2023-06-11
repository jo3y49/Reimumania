using UnityEngine;

public class ObjectInteraction : MonoBehaviour {
    private PlayerData playerData;

    private void Awake() {
        playerData = GetComponentInParent<PlayerData>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            playerData.CollectCoin(other.gameObject);
        }
        if (other.gameObject.CompareTag("Life"))
        {
            playerData.CollectLife(other.gameObject);
        }
        if (other.gameObject.CompareTag("Bomb"))
        {
            playerData.CollectBomb(other.gameObject);
        }
        if (other.gameObject.CompareTag("Stamina"))
        {
            playerData.CollectEnergy(other.gameObject);
        }
    }
}