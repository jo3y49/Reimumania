using UnityEngine;

public class NeutralPattern : BossPattern
{
    public float fluctuationAmount = .5f;

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
        int n = 9;
        float distanceBetweenBullets = arenaWidth/n;
        float x = leftLocation;
        float y = topLocation;
        Vector2 currentDirection = Vector2.down;
        float fluctuatedX = x + UnityEngine.Random.Range(-fluctuationAmount, fluctuationAmount);

        for (int i = 0; i < n; i++)
        {
            FireBullet(new Vector2(fluctuatedX,y), currentDirection, shootSpeed);
            fluctuatedX += distanceBetweenBullets;
            y *= -1;
            currentDirection *= -1;
        }
    }

}