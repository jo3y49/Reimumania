using System.Collections;
using UnityEngine;

public class EnemyData : MonoBehaviour, Shootable
{
    public GameObject coinPrefab;
    public float health = 10f;
    public int minCoins = 5;
    public int maxCoins = 10;
    public float scatterDistance = 1.0f;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
    }
    public Direction direction = Direction.Down;

    public void Shot(float bulletDamage)
    {
        health -= bulletDamage;

        if (health <= 0)
        {
            Defeated();
        }
    }

    private void Defeated()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDataManager>().AddKill();

        // Drop coins
        int numCoins = Random.Range(minCoins, maxCoins + 1);
        for (int i = 0; i < numCoins; i++)
        {
            
            // Instantiate a coin
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);            
            CoinController coinController = coin.GetComponent<CoinController>();
            // Calculate a random position within scatterDistance of the enemy's position
            Vector3 scatter = new Vector3(Random.Range(-scatterDistance, scatterDistance), Random.Range(-scatterDistance, scatterDistance), 0);

            // Start the coroutine to move the coin
            coinController.StartCoroutine(coinController.MoveToPosition(transform.position + scatter));
        }
        Destroy(gameObject);
    }
}