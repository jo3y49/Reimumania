using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float shootingRange = 10f; 
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    
    public Transform player;
    private float nextFireTime = 0f;

    private Pattern pattern;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pattern = GetComponent<Pattern>();
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within shooting range, shoot
        if (distanceToPlayer <= shootingRange && Time.time > nextFireTime)
        {
            pattern.Shoot(MakeBullet(), bulletSpeed);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private GameObject MakeBullet()
    {
        // Create a new bullet instance
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletController>().Initialize(GetComponent<Collider2D>());
            bullet.GetComponent<Renderer>().material.color = Color.red;
            return bullet;
    }
}
