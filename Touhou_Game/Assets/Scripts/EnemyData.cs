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
            // Calculate a random position within scatterDistance of the enemy's position
            Vector3 scatter = new Vector3(Random.Range(-scatterDistance, scatterDistance), Random.Range(-scatterDistance, scatterDistance), 0);
            Vector3 coinPosition = transform.position + scatter;

            // Instantiate a coin at the calculated position
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }
}