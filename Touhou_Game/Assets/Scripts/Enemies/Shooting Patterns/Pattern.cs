using UnityEngine;

public interface Pattern
{
    void Shoot(GameObject bulletPrefab, Transform player, float bulletSoeed);
}