using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Speed of character movement
    public float sprint = 1.8f; // Speed multipler for sprint
    public GameObject bulletPrefab; // Bullet prefab to be fired
    public float bulletSpeed = 10f; // Speed of bullet
    public Transform firePoint; // Point where the bullet should be fired from
    public float fireRate = 5f; // Bullets fired per second
    public float directionPersistTime = 0.1f; // Time in seconds the direction should persist


    private Vector2 moveDirection;
    private Vector2 aimDirection = Vector2.down;
    private float nextFireTime = 0f;

    private enum Direction
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

    private Direction currentDirection = Direction.Down;

    private bool[] keys = new bool[4]; // 0 - Up, 1 - Down, 2 - Left, 3 - Right
    private bool[] lastKeys = new bool[4]; // Previous frame's keys state

    private float lastKeyReleaseTime;

    private bool wasDiagonal = false; // Keeps track if the last movement was diagonal

    private float lastChangeTime;

    void Update()
    {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Get input for rotation
        keys[0] = Input.GetKey(KeyCode.UpArrow);
        keys[1] = Input.GetKey(KeyCode.DownArrow);
        keys[2] = Input.GetKey(KeyCode.LeftArrow);
        keys[3] = Input.GetKey(KeyCode.RightArrow);

        bool isDiagonal = (keys[0] || keys[1]) && (keys[2] || keys[3]); // Check if movement is diagonal
        bool wasDiagonal = (currentDirection == Direction.UpRight || currentDirection == Direction.UpLeft || currentDirection == Direction.DownRight || currentDirection == Direction.DownLeft);

        // If a key has been released and it's still diagonal, record the time
        if (wasDiagonal && ((lastKeys[0] && !keys[0]) || (lastKeys[1] && !keys[1]) || (lastKeys[2] && !keys[2]) || (lastKeys[3] && !keys[3])))
        {
            lastKeyReleaseTime = Time.time;
        }

        if (isDiagonal || !wasDiagonal || (!isDiagonal && wasDiagonal && Time.time > lastKeyReleaseTime + directionPersistTime))
        {
            if (keys[0])
            {
                if (keys[3])
                {
                    currentDirection = Direction.UpRight;
                    aimDirection = new Vector2(1, 1).normalized;
                }
                else if (keys[2])
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
            else if (keys[1])
            {
                if (keys[3])
                {
                    currentDirection = Direction.DownRight;
                    aimDirection = new Vector2(1, -1).normalized;
                }
                else if (keys[2])
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
            else if (keys[2] && !keys[3])
            {
                currentDirection = Direction.Left;
                aimDirection = new Vector2(-1, 0);
            }
            else if (keys[3] && !keys[2])
            {
                currentDirection = Direction.Right;
                aimDirection = new Vector2(1, 0);
            }

            lastChangeTime = Time.time;
        }

        // Fire bullet
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            FireBullet(aimDirection);
            nextFireTime = Time.time + 1f/fireRate;
        }

        // Apply the movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speed * sprint * Time.deltaTime;
        } else {
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;
        }

        lastKeys = (bool[])keys.Clone();
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}
