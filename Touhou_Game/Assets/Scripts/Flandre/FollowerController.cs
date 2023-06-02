using System.Collections;
using UnityEngine;

public class FollowerController : MonoBehaviour, Shootable
{
    public GameObject player; // The player object the follower should follow
    public float speed = 2f; // The speed at which the follower should follow the player
    public float energy = 100f; 
    public float energyTick = 2f;
    public float rotationSpeed = 10f; // Speed at which the object rotates
    public float dodgeSpeed = 5f; // Speed for dodge
    public float dodgeTime = .1f;
    public float restingOffset = 1f; // The height above the player where the follower rests
    public KeyCode restingKey = KeyCode.R; // The key to toggle resting
    private float maxEnergy;
    private bool isRestingKeyPressed = false;
    private Coroutine currentCoroutine;

    private enum FollowerState { Following, Transitioning, Resting }
    private FollowerState state = FollowerState.Resting;

    public float distanceFromPlayer = 2f;  // Distance from the player

    private bool canDodge = false; // State check variable
    private PlayerData playerData; // Reference to the PlayerShooting script

    private void Start() {
        playerData = player.GetComponent<PlayerData>();
        maxEnergy = energy;
        StartCoroutine(EnergyTick());
    }

    void Update()
    {
        if (Input.GetKeyDown(restingKey))
        {
            isRestingKeyPressed = true;
            
            // Toggle the follower's state when the resting key is pressed
            if (state == FollowerState.Following)
            {
                currentCoroutine = StartCoroutine(MoveToRestingPosition());
            }
            else
            if ((state == FollowerState.Resting || state == FollowerState.Transitioning) && energy >= maxEnergy/2)
            {
                // Stop the current coroutine if one is running
                if (currentCoroutine != null)
                    StopCoroutine(currentCoroutine);
                state = FollowerState.Following;
                canDodge = true;
            }
        }

        // Determine the desired rotation based on the player's direction
        Quaternion desiredRotation = Quaternion.identity;
        Vector3 desiredPosition = player.transform.position;

        switch (state)
        {
            case FollowerState.Following:
                switch (playerData.direction)
                {
                    case PlayerData.Direction.Up:
                        desiredRotation = Quaternion.Euler(0, 0, 0);
                        desiredPosition += new Vector3(0, -distanceFromPlayer, 0);
                        break;
                    case PlayerData.Direction.Down:
                        desiredRotation = Quaternion.Euler(0, 0, 180);
                        desiredPosition += new Vector3(0, distanceFromPlayer, 0);
                        break;
                    case PlayerData.Direction.Left:
                        desiredRotation = Quaternion.Euler(0, 0, 90);
                        desiredPosition += new Vector3(distanceFromPlayer, 0, 0);
                        break;
                    case PlayerData.Direction.Right:
                        desiredRotation = Quaternion.Euler(0, 0, -90);
                        desiredPosition += new Vector3(-distanceFromPlayer, 0, 0);
                        break;
                    case PlayerData.Direction.UpRight:
                        desiredRotation = Quaternion.Euler(0, 0, -45);
                        desiredPosition += new Vector3(-distanceFromPlayer, -distanceFromPlayer, 0);
                        break;
                    case PlayerData.Direction.UpLeft:
                        desiredRotation = Quaternion.Euler(0, 0, 45);
                        desiredPosition += new Vector3(distanceFromPlayer, -distanceFromPlayer, 0);
                        break;
                    case PlayerData.Direction.DownRight:
                        desiredRotation = Quaternion.Euler(0, 0, -135);
                        desiredPosition += new Vector3(-distanceFromPlayer, distanceFromPlayer, 0);
                        break;
                    case PlayerData.Direction.DownLeft:
                        desiredRotation = Quaternion.Euler(0, 0, 135);
                        desiredPosition += new Vector3(distanceFromPlayer, distanceFromPlayer, 0);
                        break;
                }

                // Interpolate the follower's rotation towards the desired rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
                Vector3 direction = (desiredPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                break;
            case FollowerState.Resting:
                transform.position = desiredPosition + new Vector3(0, restingOffset, 0);
                break;
        }

        
    }

    public void Shot(float bulletDamage)
    {
        if (canDodge)
        {
            

            if (energy > bulletDamage)
            {
                energy -= bulletDamage;
                // Play the dodging animation
                StartCoroutine(DodgeBullet());
            }
            else
            {
                energy = 0;
                currentCoroutine = StartCoroutine(MoveToRestingPosition());
            }
        }
    }

    private IEnumerator MoveToRestingPosition()
    {
        state = FollowerState.Transitioning;
        canDodge = false;
        Vector3 restingPosition = player.transform.position + new Vector3(0, restingOffset, 0);
        while (Vector3.Distance(transform.position, restingPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, restingPosition, speed * Time.deltaTime);
            restingPosition = player.transform.position + new Vector3(0, restingOffset, 0);
            yield return null;
        }
        state = FollowerState.Resting;
        currentCoroutine = null;
    }

    private IEnumerator DodgeBullet()
    {
        canDodge = false;

        // Dodge direction (perpendicular to the direction to player)
        Vector3 dodgeDirection = Vector3.Cross((transform.position - player.transform.position).normalized, Vector3.forward);

        // Dodge movement: Move sideways quickly and then slowly return to following the player
        float startTime = Time.time;
        while(Time.time < startTime + dodgeTime && !isRestingKeyPressed)
        {
            // Move in the dodge direction
            transform.position += dodgeDirection * dodgeSpeed * Time.deltaTime;

            yield return null;
        }

        // Reset resting key pressed flag and only enable dodging if the coroutine wasn't interrupted
        isRestingKeyPressed = false;
        if (!isRestingKeyPressed) canDodge = true;
    }
    private IEnumerator EnergyTick()
    {
        while (true)
        {
            if (state == FollowerState.Following)
            {
                energy--;
                if (energy <= 0)
                {
                    currentCoroutine = StartCoroutine(MoveToRestingPosition());
                }
            }
            else if (state == FollowerState.Resting)
            {
                if (energy < maxEnergy)
                {
                    energy++;
                }
            }

            yield return new WaitForSeconds(energyTick);
        }
    }
}
