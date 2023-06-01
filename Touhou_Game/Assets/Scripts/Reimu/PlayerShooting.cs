using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab to be fired
    public Transform firePoint; // Point where the bullet should be fired from
    public float directionPersistTime = 0.1f; // Time in seconds the direction should persist
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
    }

    public Direction currentDirection = Direction.Down;

    private Vector2 aimDirection = Vector2.down;
    private float nextFireTime = 0f;

    private bool[] currentKeyStates = new bool[4]; // 0 - Up, 1 - Down, 2 - Left, 3 - Right
    private bool[] previousKeyStates = new bool[4]; // Previous frame's keys state

    private float lastKeyReleaseTime;

    private float lastChangeTime;

    void Update()
    {

        // Get input for rotation
        UpdateKeyStates();

        bool isDiagonal = IsDiagonalMovement();
        bool wasDiagonal = IsCurrentDirectionDiagonal();

        if (wasDiagonal && DidKeyReleaseCauseDiagonal())
        {
            lastKeyReleaseTime = Time.time;
        }

        if (ShouldChangeDirection(isDiagonal, wasDiagonal))
        {
            ChangeDirection();
        }

        // Fire bullet
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            FireBullet(aimDirection);
            nextFireTime = Time.time + 1f/fireRate;
        }

        previousKeyStates = (bool[])currentKeyStates.Clone();
    }

    private void UpdateKeyStates()
    {
        currentKeyStates[0] = Input.GetKey(KeyCode.UpArrow);
        currentKeyStates[1] = Input.GetKey(KeyCode.DownArrow);
        currentKeyStates[2] = Input.GetKey(KeyCode.LeftArrow);
        currentKeyStates[3] = Input.GetKey(KeyCode.RightArrow);
    }

    private bool IsDiagonalMovement()
    {
        return (currentKeyStates[0] || currentKeyStates[1]) && (currentKeyStates[2] || currentKeyStates[3]);
    }

    private bool IsCurrentDirectionDiagonal()
    {
        return (currentDirection == Direction.UpRight || currentDirection == Direction.UpLeft || currentDirection == Direction.DownRight || currentDirection == Direction.DownLeft);
    }

    private bool DidKeyReleaseCauseDiagonal()
    {
        for (int i = 0; i < 4; i++)
        {
            if (previousKeyStates[i] && !currentKeyStates[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool ShouldChangeDirection(bool isDiagonal, bool wasDiagonal)
    {
        return isDiagonal || !wasDiagonal || (!isDiagonal && wasDiagonal && Time.time > lastKeyReleaseTime + directionPersistTime);
    }

    private void ChangeDirection()
    {
        if (currentKeyStates[0])
            {
                if (currentKeyStates[3])
                {
                    currentDirection = Direction.UpRight;
                    aimDirection = new Vector2(1, 1).normalized;
                }
                else if (currentKeyStates[2])
                {
                    currentDirection = Direction.UpLeft;
                    aimDirection = new Vector2(-1, 1).normalized;
                }
                else
                {
                    currentDirection = Direction.Up;
                    aimDirection = new Vector2(0, 1);
                }
            }
            else if (currentKeyStates[1])
            {
                if (currentKeyStates[3])
                {
                    currentDirection = Direction.DownRight;
                    aimDirection = new Vector2(1, -1).normalized;
                }
                else if (currentKeyStates[2])
                {
                    currentDirection = Direction.DownLeft;
                    aimDirection = new Vector2(-1, -1).normalized;
                }
                else
                {
                    currentDirection = Direction.Down;
                    aimDirection = new Vector2(0, -1);
                }
            }
            else if (currentKeyStates[2] && !currentKeyStates[3])
            {
                currentDirection = Direction.Left;
                aimDirection = new Vector2(-1, 0);
            }
            else if (currentKeyStates[3] && !currentKeyStates[2])
            {
                currentDirection = Direction.Right;
                aimDirection = new Vector2(1, 0);
            }

            lastChangeTime = Time.time;
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<BulletController>().Initialize(GetComponent<Collider2D>());
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}
