using UnityEngine;

public class FollowerDefense : MonoBehaviour {
    public FollowerController followerController;
    
    private void Awake() {
        followerController = GetComponent<FollowerController>();
    }
    private void OnEnable() {
        BulletController.ProtectPlayer += DeflectBullet;
    }

    private void OnDisable() {
        BulletController.ProtectPlayer -= DeflectBullet;
    }

    private void DeflectBullet(GameObject bullet)
    {
        Debug.Log("detect");
    }
}