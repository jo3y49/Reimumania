using UnityEngine;

public class EnemyController : MonoBehaviour, Shootable
{
    public Transform player; // Assign your player in the Inspector
    public GameObject bulletPrefab; // Assign your bullet prefab in the Inspector
    public GameObject coinPrefab;
    public float shootingRange = 10f; // Assign your desired shooting range in the Inspector
    public float bulletSpeed = 10f;
    public float fireRate = 5;
    public float health = 10f;
    public int minCoins = 5;
    public int maxCoins = 10;
    public float scatterDistance = 1.0f;


    private float nextFireTime = 0f;


    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within shooting range, shoot
        if (distanceToPlayer <= shootingRange && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f/fireRate;
        }
    }

    void Shoot()
    {
        // Create a new bullet instance
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletController>().Initialize(GetComponent<Collider2D>());
        bullet.GetComponent<Renderer>().material.color = Color.red;

        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Adjust the bullet's velocity to shoot towards the player
        bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer * bulletSpeed;

        // Adjust the bullet's direction to face the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
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
