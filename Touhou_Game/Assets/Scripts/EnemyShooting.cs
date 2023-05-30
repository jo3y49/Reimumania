using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float shootingRange = 10f; 
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    
    private Transform player;
    private float nextFireTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within shooting range, shoot
        if (distanceToPlayer <= shootingRange && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
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
}
