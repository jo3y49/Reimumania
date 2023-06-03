using UnityEngine;

public class ShootForward : MonoBehaviour, Pattern {
    public void Shoot(GameObject bullet, Transform player, float bulletSpeed)
    {
        Vector2 aimDirection = new Vector2(0,0);

        switch (GetComponent<EnemyData>().direction)
        {
            case EnemyData.Direction.Up:
            aimDirection = new Vector2(0, 1);
            break;
            case EnemyData.Direction.Down:
            aimDirection = new Vector2(0, -1);
            break;
            case EnemyData.Direction.Left:
            aimDirection = new Vector2(-1, 0);
            break;
            case EnemyData.Direction.Right:
            aimDirection = new Vector2(1, 0);
            break;
            case EnemyData.Direction.UpRight:
            aimDirection = new Vector2(1, 1).normalized;
            break;
            case EnemyData.Direction.UpLeft:
            aimDirection = new Vector2(-1, 1).normalized;
            break;
            case EnemyData.Direction.DownRight:
            aimDirection = new Vector2(1, -1).normalized;
            break;
            case EnemyData.Direction.DownLeft:
            aimDirection = new Vector2(-1, -1).normalized;
            break;

        }

        bullet.GetComponent<Rigidbody2D>().velocity = aimDirection * bulletSpeed;
    }
}