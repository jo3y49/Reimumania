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

    private void Awake() {
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        pattern = GetComponent<Pattern>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<PlayerData>();
    }

    private void Update()
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
