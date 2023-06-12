using UnityEngine;

public class NeutralPattern : BossPattern
{
    private void Update() {
        Shoot();
    }

    private void Shoot()
    {
        if (CanShoot(nextFireTime))
        {
            FireBullet(transform.position, Vector2.down, shootSpeed);
            nextFireTime = Time.time + 1f/fireRate;
        }
    }
}