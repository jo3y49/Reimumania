using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float shootingRange = 10f; 
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    
    private GameObject player;
    private float nextFireTime = 0f;
    private Collider2D enemyCollider;
    private PlayerData playerData;
    private Pattern pattern;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pattern = GetComponent<Pattern>();
        enemyCollider = GetComponent<Collider2D>();
        playerData = player.GetComponent<PlayerData>();
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player is within shooting range, shoot
        if (distanceToPlayer <= shootingRange && Time.time >= nextFireTime && player.GetComponent<PlayerData>().isHittable)
        {
            pattern.Shoot(MakeBullet(), player.transform, bulletSpeed);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private GameObject MakeBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Initialize(enemyCollider);
        bulletController.GetComponent<Renderer>().material.color = Color.red;
        return bullet;
    }
}
