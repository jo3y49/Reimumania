using UnityEngine;

public class NeutralPattern : BossPattern
{   
    public int numberOfBullets = 8;
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
        float distanceBetweenBullets = arenaWidth/numberOfBullets;
        float x = leftLocation;
        float y = topLocation;
        Vector2 currentDirection = Vector2.down;
        float fluctuatedX = x + UnityEngine.Random.Range(-fluctuationAmount, fluctuationAmount);

        for (int i = 0; i < numberOfBullets; i++)
        {
            FireBullet(new Vector2(fluctuatedX,y), currentDirection, shootSpeed);
            fluctuatedX += distanceBetweenBullets;
            y *= -1;
            currentDirection *= -1;
        }
    }

}