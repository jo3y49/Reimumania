using UnityEngine;

public class FollowerDefense : MonoBehaviour, FollowerAction {
    public FollowerController followerController;
    public int energyDrain = 50;
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
    private void OnEnable() {
        BulletController.protectPlayer += DeflectBullet;
    }

    private void OnDisable() {
        BulletController.protectPlayer -= DeflectBullet;
    }

    private void DeflectBullet(GameObject bullet)
    {
        followerController.SetIsActing();

        transform.position = bullet.transform.position;

        bullet.GetComponent<BulletController>().Reflect(playerCollider);
        
        followerController.energy -= energyDrain;

        followerController.SetNotActing();
    }
}