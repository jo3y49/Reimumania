using UnityEngine;

public class FollowerDefense : MonoBehaviour {
    public FollowerController followerController;
    private Collider2D playerCollider;
    
    private void Awake() {
        followerController = GetComponent<FollowerController>();
    }
    private void Start() {
        playerCollider = GameObject.FindGameObjectWithTag("Hit Box").GetComponent<Collider2D>();
    }
    private void OnEnable() {
        BulletController.ProtectPlayer += DeflectBullet;
    }

    private void OnDisable() {
        BulletController.ProtectPlayer -= DeflectBullet;
    }

    private void DeflectBullet(GameObject bullet)
    {
        transform.position = bullet.transform.position;

        bullet.GetComponent<BulletController>().Reflect(playerCollider);
        
        followerController.energy -= 45;
    }
}