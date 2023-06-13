using UnityEngine;

public class CirclePattern : BossPattern {
    public int numberOfBullets = 10;

    private void Update() {
        Shoot();
    }

    private void Shoot()
    {
        if (CanShoot(nextFireTime))
        {

            MakePattern();
            nextFireTime = Time.time + 1f/fireRate;
        }
    }

    private void MakePattern()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * 360f / numberOfBullets;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            FireBullet(transform.position, direction, shootSpeed);
        }
    }
}