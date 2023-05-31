using System.Collections;
using UnityEngine;

public class FollowerController : MonoBehaviour, Shootable
{
    public GameObject player; // The player object the follower should follow
    public float speed = 2f; // The speed at which the follower should follow the player
    public float energy = 10f; 
    public float rotationSpeed = 10f; // Speed at which the object rotates
    public float dodgeSpeed = 5f; // Speed for dodge
    public float dodgeTime = .1f;

    public float distanceFromPlayer = 2f;  // Distance from the player

    private bool isDodging = false; // State check variable
    private PlayerShooting playerShooting; // Reference to the PlayerShooting script

    private void Start() {
        playerShooting = player.GetComponent<PlayerShooting>();
    }

    void Update()
    {
        // Determine the desired rotation based on the player's direction
        Quaternion desiredRotation = Quaternion.identity;
        Vector3 desiredPosition = player.transform.position;

        switch (playerShooting.currentDirection)
        {
            case PlayerShooting.Direction.Up:
                desiredRotation = Quaternion.Euler(0, 0, 0);
                desiredPosition += new Vector3(0, -distanceFromPlayer, 0);
                break;
            case PlayerShooting.Direction.Down:
                desiredRotation = Quaternion.Euler(0, 0, 180);
                desiredPosition += new Vector3(0, distanceFromPlayer, 0);
                break;
            case PlayerShooting.Direction.Left:
                desiredRotation = Quaternion.Euler(0, 0, 90);
                desiredPosition += new Vector3(distanceFromPlayer, 0, 0);
                break;
            case PlayerShooting.Direction.Right:
                desiredRotation = Quaternion.Euler(0, 0, -90);
                desiredPosition += new Vector3(-distanceFromPlayer, 0, 0);
                break;
            case PlayerShooting.Direction.UpRight:
                desiredRotation = Quaternion.Euler(0, 0, -45);
                desiredPosition += new Vector3(-distanceFromPlayer, -distanceFromPlayer, 0);
                break;
            case PlayerShooting.Direction.UpLeft:
                desiredRotation = Quaternion.Euler(0, 0, 45);
                desiredPosition += new Vector3(distanceFromPlayer, -distanceFromPlayer, 0);
                break;
            case PlayerShooting.Direction.DownRight:
                desiredRotation = Quaternion.Euler(0, 0, -135);
                desiredPosition += new Vector3(-distanceFromPlayer, distanceFromPlayer, 0);
                break;
            case PlayerShooting.Direction.DownLeft:
                desiredRotation = Quaternion.Euler(0, 0, 135);
                desiredPosition += new Vector3(distanceFromPlayer, distanceFromPlayer, 0);
                break;
        }
        Vector3 direction = (desiredPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Interpolate the follower's rotation towards the desired rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }

    public void Shot(float bulletDamage)
    {
        if (energy > bulletDamage)
            energy -= bulletDamage;
        else 
            energy = 0;

        // Play the shooting animation
        StartCoroutine(DodgeBullet());
    }

    IEnumerator DodgeBullet()
    {
        // If already dodging, don't start another dodge
        if (isDodging) yield break;

        isDodging = true;

        // Dodge direction (perpendicular to the direction to player)
        Vector3 dodgeDirection = Vector3.Cross((transform.position - player.transform.position).normalized, Vector3.forward);

        // Dodge movement: Move sideways quickly and then slowly return to following the player
        float startTime = Time.time;
        while(Time.time < startTime + dodgeTime)
        {
            // Move in the dodge direction
            transform.position += dodgeDirection * dodgeSpeed * Time.deltaTime;

            yield return null;
        }

        isDodging = false;
    }
}
