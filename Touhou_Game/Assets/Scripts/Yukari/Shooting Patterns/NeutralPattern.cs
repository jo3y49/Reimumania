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

            MakePattern();
            nextFireTime = Time.time + 1f/fireRate;
        }
    }

    private void MakePattern()
    {
        int n = 1;
        float distanceBetweenBullets = arenaWidth/n;
        float x = leftLocation;
        float y = topLocation;
        Vector2 currentDirection = Vector2.down;

        for (int i = 0; i < n; i++)
        {
            FireBullet(new Vector2(x,y), currentDirection, shootSpeed);
            x += distanceBetweenBullets;
            y *= -1;
            currentDirection *= -1;
        }
    }

}