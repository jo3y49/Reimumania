using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign your bullet prefab in the Inspector
    public Transform player; // Assign your player in the Inspector
    public float shootingRange = 10f; // Assign your desired shooting range in the Inspector

    private float nextFireTime = 0f;


    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within shooting range, shoot
        if (distanceToPlayer <= shootingRange && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f/BulletData.Instance.fireRate;
        }
    }

    void Shoot()
    {
        // Create a new bullet instance
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletController>().Initialize(GetComponent<Collider2D>());

        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Adjust the bullet's velocity to shoot towards the player
        bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer * BulletData.Instance.bulletSpeed;

        // Adjust the bullet's direction to face the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
