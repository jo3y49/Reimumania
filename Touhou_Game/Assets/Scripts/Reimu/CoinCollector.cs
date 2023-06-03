using UnityEngine;

public class CoinCollector : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            transform.parent.GetComponent<PlayerData>().CollectCoin(other.gameObject);
        }
    }
}