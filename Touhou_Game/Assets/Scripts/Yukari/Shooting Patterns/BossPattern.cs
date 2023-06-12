using UnityEngine;

public abstract class BossPattern : MonoBehaviour {

    public float shootSpeed, fireRate, nextFireTime;
    public float arenaWidth, arenaHeight, leftLocation, rightLocation, topLocation, bottomLocation;
    public GameObject bulletPrefab;
    public GameObject portalPrefab;
    public Collider2D bossCollider;
    public BossData bossData;

    public void Awake() {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        bossCollider = boss.GetComponent<Collider2D>();
        bossData = boss.GetComponent<BossData>();
        nextFireTime = 0;
        leftLocation = -arenaWidth/2;
        rightLocation = arenaWidth/2;
        topLocation = arenaHeight/2;
        bottomLocation = -arenaHeight/2;
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