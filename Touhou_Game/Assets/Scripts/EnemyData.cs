using System.Collections;
using UnityEngine;

public class EnemyData : MonoBehaviour, Shootable
{
    public GameObject coinPrefab;
    public float health = 10f;
    public int minCoins = 5;
    public int maxCoins = 10;
    public float scatterDistance = 1.0f;

    public void Shot(float bulletDamage)
    {
        if (health > bulletDamage)
            health -= bulletDamage;
        else 
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Drop coins
        int numCoins = Random.Range(minCoins, maxCoins + 1);
        for (int i = 0; i < numCoins; i++)
        {
            
            // Instantiate a coin
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            
            // Calculate a random position within scatterDistance of the enemy's position
            Vector3 scatter = new Vector3(Random.Range(-scatterDistance, scatterDistance), Random.Range(-scatterDistance, scatterDistance), 0);

            // Start the coroutine to move the coin
            coin.GetComponent<CoinController>().StartCoroutine(coin.GetComponent<CoinController>().MoveToPosition(transform.position + scatter));

        }
    }
}