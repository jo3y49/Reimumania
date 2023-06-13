using UnityEngine;

public abstract class BossPattern : MonoBehaviour {

    public float shootSpeed, fireRate, nextFireTime;
    public float arenaWidth, arenaHeight, leftLocation, rightLocation, topLocation, bottomLocation;
    public Transform playerLocation;
    public GameObject bulletPrefab;
    public GameObject portalPrefab;
    public Collider2D bossCollider;
    public BossData bossData;

    protected void Start() {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        bossCollider = boss.GetComponent<Collider2D>();
        bossData = boss.GetComponent<BossData>();
        bulletPrefab = bossData.bulletPrefab;
        portalPrefab = bossData.portalPrefab;
        playerLocation = bossData.playerLocation;
        nextFireTime = 0;
        arenaHeight = bossData.arenaHeight;
        arenaWidth = bossData.arenaWidth;
        leftLocation = bossData.leftLocation;
        rightLocation = bossData.rightLocation;
        topLocation = bossData.topLocation;
        bottomLocation = bossData.bottomLocation;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }
    
    public GameObject FireBullet(Vector2 firePosition, Vector2 fireDirection, float shootSpeed)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePosition, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Initialize(bossCollider);
        bullet.GetComponent<Rigidbody2D>().velocity = fireDirection * shootSpeed;
        return bullet;
    }

    public bool CanShoot(float nextFireTime)
    {
        return Time.time > nextFireTime;
    }
}