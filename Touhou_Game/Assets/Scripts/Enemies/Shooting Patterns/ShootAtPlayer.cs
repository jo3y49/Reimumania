using UnityEngine;

public class ShootAtPlayer : MonoBehaviour, Pattern
{
    public void Shoot(GameObject bullet, float bulletSpeed)
    {
        // Calculate direction to the player
        Vector3 shootDirection = (GetComponent<EnemyShooting>().player.position - transform.position).normalized;

        // Adjust the bullet's velocity to shoot towards the player
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

        // Adjust the bullet's direction to face the player
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
