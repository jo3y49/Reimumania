using UnityEngine;

public class FollowerAssist : MonoBehaviour, FollowerAction {

    public GameObject bulletPrefab;
    public FollowerController followerController;
    public float bulletOffset = 1f;
    public int energyDrain = 2;
    private Collider2D playerCollider;
    
    private void Awake() {
        followerController = GetComponent<FollowerController>();
    }

    private void Start() {
        playerCollider = GameObject.FindGameObjectWithTag("Hit Box").GetComponent<Collider2D>();
    }

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        PlayerShooting.shootAssist += BulletUpgrade;
    }
    private void OnDisable()
    {
        PlayerShooting.shootAssist -= BulletUpgrade;
    }


    private void BulletUpgrade(GameObject bullet)
    {
        CreateUpgrade(bullet, new Vector3(bulletOffset,0,0));
        CreateUpgrade(bullet, new Vector3(-bulletOffset,0,0));
        CreateUpgrade(bullet, new Vector3(0,bulletOffset,0));
        CreateUpgrade(bullet, new Vector3(0,-bulletOffset,0));

        followerController.EnergyDecrease(energyDrain);
    }

    private void CreateUpgrade(GameObject bullet, Vector3 offset)
    {
        GameObject assistBullet = Instantiate(bulletPrefab, bullet.transform.position + offset, bullet.transform.rotation);
        assistBullet.GetComponent<BulletController>().Initialize(playerCollider);
        assistBullet.GetComponent<Rigidbody2D>().velocity = bullet.GetComponent<Rigidbody2D>().velocity;
    }
}