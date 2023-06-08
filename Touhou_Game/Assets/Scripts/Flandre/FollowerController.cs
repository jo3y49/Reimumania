using System.Collections;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    private GameObject player; // The player object the follower should follow
    public float speed = 2f; // The speed at which the follower should follow the player
    public float energy = 100f; 
    public float energyTick = 2f;
    public float rotationSpeed = 10f; // Speed at which the object rotates
    public float dodgeSpeed = 5f; // Speed for dodge
    public float dodgeTime = .1f;
    public float dodgeStaminaDrain = 5f;
    public float distanceFromPlayer = 2f;  // Distance from the player
    public float restingOffset = 1f; // The height above the player where the follower rests
    public KeyCode restingKey = KeyCode.R; // The key to toggle resting
    private float maxEnergy;
    private Coroutine currentCoroutine;

    private enum FollowerState { Following, Transitioning, Resting }
    [SerializeField] private FollowerState state = FollowerState.Resting;

    private PlayerData playerData; // Reference to the PlayerShooting script

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<PlayerData>();
        maxEnergy = energy;
        StartCoroutine(EnergyTick());
    }

    private void Update()
    {
        if (Input.GetKeyDown(restingKey) && currentCoroutine == null)
        {            
            // Toggle the follower's state when the resting key is pressed
            if (state == FollowerState.Following)
            {
                StartCoroutine(MoveToRestingPosition());
            }
            else
            if ((state == FollowerState.Resting || state == FollowerState.Transitioning) && energy >= maxEnergy/2)
            {
                // Stop the current coroutine if one is running
                state = FollowerState.Following;
            }
        }

        FollowPlayer();
    }

    private void FollowPlayer()
    {
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
                float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredPosition);

                if (distanceToDesiredPosition > .01f)
                {
                    Vector3 direction = (desiredPosition - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;
                }
                break;
            case FollowerState.Resting:
                transform.position = desiredPosition + new Vector3(0, restingOffset, 0);
                break;
        }
    }

    public void Dodge(Transform bullet)
    {
        if (currentCoroutine == null && state == FollowerState.Following)
        {
            currentCoroutine = StartCoroutine(DodgeBullet(bullet));
        }
    }

    private IEnumerator MoveToRestingPosition()
    {
        state = FollowerState.Transitioning;
        Vector3 restingPosition = player.transform.position + new Vector3(0, restingOffset, 0);
        while (Vector3.Distance(transform.position, restingPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, restingPosition, speed * Time.deltaTime);
            restingPosition = player.transform.position + new Vector3(0, restingOffset, 0);
            yield return null;
        }
        state = FollowerState.Resting;
    }

    private IEnumerator DodgeBullet(Transform bullet)
    {
        energy -= dodgeStaminaDrain;

        // Dodge direction (perpendicular to the direction to player)
        Vector3 dodgeDirection = Vector3.Cross((transform.position - bullet.position).normalized, Vector3.back);

        // Dodge movement: Move sideways quickly and then slowly return to following the player
        float startTime = Time.time;
        while(Time.time < startTime + dodgeTime)
        {
            // Move in the dodge direction
            transform.position += dodgeDirection * dodgeSpeed * Time.deltaTime;

            yield return null;
        }

        currentCoroutine = null;

        if (energy <= 0)
        {
            energy = 0;
            StartCoroutine(MoveToRestingPosition());
        }
    }
    private IEnumerator EnergyTick()
    {
        while (true)
        {
            if (currentCoroutine == null)
            {
                if (state == FollowerState.Following)
                {
                    energy -= 1;
                    if (energy <= 0)
                    {
                        StartCoroutine(MoveToRestingPosition());
                    }
                }
                else if (state == FollowerState.Resting)
                {
                    if (energy < maxEnergy)
                    {
                        energy += 1;
                    }
                }
            }

            yield return new WaitForSeconds(energyTick);
        }
    }
}
