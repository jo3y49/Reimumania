using System.Collections;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    private GameObject player; // The player object the follower should follow
    public float speed = 2f; // The speed at which the follower should follow the player
    public int energy = 100; 
    public int energyTick = 2;
    public float rotationSpeed = 10f; // Speed at which the object rotates
    public float dodgeSpeed = 5f; // Speed for dodge
    public float dodgeTime = .1f;
    public int dodgeStaminaDrain = 5;
    public Vector2 distanceFromPlayer = new Vector2(1,1);  // Distance from the player
    public float restingOffset = 1f; // The height above the player where the follower rests
    public KeyCode restingKey = KeyCode.R; // The key to toggle resting
    private int maxEnergy;
    private Coroutine dodgingCoroutine;
    private Vector3 currentPosition;

    private enum FollowState { Following, Transitioning, Resting }
    private enum ActionState { Attack, Defend }
    [SerializeField] private FollowState Followstate = FollowState.Resting;
    [SerializeField] private ActionState actionState = ActionState.Defend;

    private PlayerData playerData; // Reference to the PlayerShooting script

    private void Start() {
        currentPosition = new Vector3(-distanceFromPlayer.x,distanceFromPlayer.y,0);
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<PlayerData>();
        maxEnergy = energy;
        StartCoroutine(EnergyTick());
    }

    private void Update()
    {
        if (Input.GetKeyDown(restingKey) && dodgingCoroutine == null)
        {            
            // Toggle the follower's state when the resting key is pressed
            if (Followstate == FollowState.Following)
            {
                MoveToRestingPosition();
            }
            else
            if ((Followstate == FollowState.Resting || Followstate == FollowState.Transitioning) && energy >= maxEnergy/2)
            {
                // Stop the current coroutine if one is running
                Followstate = FollowState.Following;
            }
        }
    }

    private void FixedUpdate() {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // Determine the desired rotation based on the player's direction
        Quaternion desiredRotation = Quaternion.identity;

        switch (Followstate)
        {
            case FollowState.Following:
                switch (playerData.direction)
                {
                    case PlayerData.Direction.Up:
                        desiredRotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case PlayerData.Direction.Down:
                        desiredRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    case PlayerData.Direction.Left:
                        desiredRotation = Quaternion.Euler(0, 0, 90);
                        currentPosition = new Vector3(distanceFromPlayer.x, distanceFromPlayer.y, 0);
                        break;
                    case PlayerData.Direction.Right:
                        desiredRotation = Quaternion.Euler(0, 0, -90);
                        currentPosition = new Vector3(-distanceFromPlayer.x, distanceFromPlayer.y, 0);
                        break;
                    case PlayerData.Direction.UpRight:
                        desiredRotation = Quaternion.Euler(0, 0, -45);
                        currentPosition = new Vector3(-distanceFromPlayer.x, distanceFromPlayer.y, 0);
                        break;
                    case PlayerData.Direction.UpLeft:
                        desiredRotation = Quaternion.Euler(0, 0, 45);
                        currentPosition = new Vector3(distanceFromPlayer.x, distanceFromPlayer.y, 0);
                        break;
                    case PlayerData.Direction.DownRight:
                        desiredRotation = Quaternion.Euler(0, 0, -135);
                        break;
                    case PlayerData.Direction.DownLeft:
                        desiredRotation = Quaternion.Euler(0, 0, 135);
                        break;
                }
                Vector3 desiredPosition = player.transform.position + currentPosition;

                // Interpolate the follower's rotation towards the desired rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
                float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredPosition);

                if (distanceToDesiredPosition > .12f)
                {
                    Vector3 direction = (desiredPosition - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;
                } else {
                    transform.position = desiredPosition;
                }
                break;
            case FollowState.Transitioning:
                Vector3 restingPosition = player.transform.position + new Vector3(0, restingOffset, 0);
                if (Vector3.Distance(transform.position, restingPosition) > 0.12f)
                {
                    Vector3 direction = (restingPosition - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;
                    restingPosition = player.transform.position + new Vector3(0, restingOffset, 0);
                } else {
                    Followstate = FollowState.Resting;
                }
                break;
            case FollowState.Resting:
                transform.position = player.transform.position + new Vector3(0, restingOffset, 0);
                break;
        }
    }

    public void Dodge(Transform bullet)
    {
        if (dodgingCoroutine == null && Followstate == FollowState.Following)
        {
            dodgingCoroutine = StartCoroutine(DodgeBullet(bullet));
        }
    }

    private void MoveToRestingPosition()
    {
        Followstate = FollowState.Transitioning;
        transform.rotation = Quaternion.Euler(0, 0, 0);
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

        dodgingCoroutine = null;

        if (energy <= 0)
        {
            energy = 0;
            MoveToRestingPosition();
        }
    }
    private IEnumerator EnergyTick()
    {
        while (true)
        {
            if (dodgingCoroutine == null)
            {
                if (Followstate == FollowState.Following)
                {
                    energy -= 1;
                    if (energy <= 0)
                    {
                        MoveToRestingPosition();
                    }
                }
                else if (Followstate == FollowState.Resting)
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
