using System.Collections;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    public GameObject player; // The player object the follower should follow
    public float speed = 2f; // The speed at which the follower should follow the player
    public int energy = 100; 
    public int energyTick = 2;
    public int energyRegain = 20;
    public int energyForAction = 25;
    public float rotationSpeed = 10f; // Speed at which the object rotates
    public float dodgeSpeed = 5f; // Speed for dodge
    public float dodgeTime = .1f;
    public int dodgeStaminaDrain = 5;
    public Vector2 distanceFromPlayer = new Vector2(1,1);  // Distance from the player
    public float restingOffset = 1f; // The height above the player where the follower rests
    public KeyCode restingKey = KeyCode.Z; // The key to toggle resting
    public KeyCode modeKey = KeyCode.R;
    private bool isActing = false;
    private int maxEnergy;
    private Coroutine dodgingCoroutine;
    private Vector3 currentPosition;

    private enum FollowState { Following, Transitioning, Resting }
    public enum ActionState { Attack, Defend, Mounted, Tired }
    private FollowState followState;
    [SerializeField] private ActionState actionState = ActionState.Defend;
    private ActionState previousActionState;
    private ActionState previousMountState;
    private FollowerAction followerAttack, followerDefense, followerAssist;

    private PlayerData playerData; // Reference to the PlayerShooting script

    private void Awake() {
        followerAttack = GetComponent<FollowerAttack>();
        followerDefense = GetComponent<FollowerDefense>();
        followerAssist = GetComponent<FollowerAssist>();
    }

    private void Start() {
        currentPosition = new Vector3(-distanceFromPlayer.x,distanceFromPlayer.y,0);
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<PlayerData>();
        maxEnergy = energy;
        InitializeStates();
        StartCoroutine(EnergyTick());
    }

    private void InitializeStates()
    {
        switch (actionState)
        {
            case ActionState.Attack:
            case ActionState.Defend:
                previousMountState = ActionState.Tired;
                ActivateAction(actionState);
                followState = FollowState.Following;
            break;
            case ActionState.Mounted:
            case ActionState.Tired:
                previousActionState = ActionState.Defend;
                ActivateMount(actionState);
                followState = FollowState.Resting;
            break;
        }
    }

    private void Update()
    {
        if (dodgingCoroutine == null && !isActing)
        {
            if (Input.GetKeyDown(restingKey))
            {
                // Toggle the follower's state when the resting key is pressed
                if (followState == FollowState.Following)
                {
                    MoveToRestingPosition();
                }
                else if ((followState == FollowState.Resting || followState == FollowState.Transitioning) && energy >= maxEnergy/2)
                {
                    followState = FollowState.Following;
                    ActivateAction(previousActionState);
                }
            } 
            else if (Input.GetKeyDown(modeKey))
            {
                if (followState == FollowState.Following)
                {
                    if (actionState == ActionState.Defend)
                    {
                        ActivateAction(ActionState.Attack);
                        
                    }
                    else if (actionState == ActionState.Attack)
                    {
                        ActivateAction(ActionState.Defend);
                    }
                }
                else if (followState == FollowState.Resting)
                {
                    if (actionState == ActionState.Mounted)
                    {
                        ActivateMount(ActionState.Tired);
                    }
                    else if (actionState == ActionState.Tired)
                    {
                        ActivateMount(ActionState.Mounted);
                    }
                }
            }
        }           
            
    }

    private void FixedUpdate() {
        if (!isActing)
            FollowPlayer();
    }

    private void ActivateAction(ActionState actionState)
    {
        followerAssist.Deactivate();

        switch (actionState)
        {
            case ActionState.Attack:
            followerDefense.Deactivate();
            followerAttack.Activate();
            break;
            case ActionState.Defend:
            followerAttack.Deactivate();
            followerDefense.Activate();
            break;
        }

        this.actionState = actionState;
        previousActionState = actionState;
        
    }

    private void ActivateMount(ActionState actionState)
    {
        followerAttack.Deactivate();
        followerDefense.Deactivate();

        switch (actionState)
        {
            case ActionState.Mounted:
            if (energy > energyForAction)
                followerAssist.Activate();
            else 
                actionState = ActionState.Tired;
            break;
            case ActionState.Tired:
            followerAssist.Deactivate();
            break;
        }

        this.actionState = actionState;
        previousMountState = actionState;
    }

    private void FollowPlayer()
    {
        // Determine the desired rotation based on the player's direction
        Quaternion desiredRotation = Quaternion.identity;

        switch (followState)
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
                // transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
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
                    followState = FollowState.Resting;
                    ActivateMount(previousMountState);
                }
                break;
            case FollowState.Resting:
                transform.position = player.transform.position + new Vector3(0, restingOffset, 0);
                break;
        }
    }

    public void Dodge(Transform bullet)
    {
        if (dodgingCoroutine == null && followState == FollowState.Following && !isActing)
        {
            dodgingCoroutine = StartCoroutine(DodgeBullet(bullet));
        }
    }

    private void MoveToRestingPosition()
    {
        followState = FollowState.Transitioning;
        actionState = ActionState.Tired;
        followerAttack.Deactivate();
        followerDefense.Deactivate();
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

        EnergyDecrease(0);
    }
    private IEnumerator EnergyTick()
    {
        while (true)
        {
            if (dodgingCoroutine == null)
            {
                if (followState == FollowState.Following)
                {
                    EnergyDecrease();
                }
                else if (followState == FollowState.Resting)
                {
                    EnergyIncrease();
                }
            }

            yield return new WaitForSeconds(energyTick);
        }
    }

    public void EnergyDecrease(int energyChange = 1)
    {
        if (!isActing)
        {
            energy -= energyChange;
            if (energy <= energyForAction)
            {
                actionState = ActionState.Tired;
                followerAttack.Deactivate();
                followerDefense.Deactivate();
                followerAssist.Deactivate();
            }

            if (energy <= 0)
            {
                energy = 0;
                MoveToRestingPosition();
            }
        }
    }

    private void EnergyIncrease(int energyChange = 1)
    {
        if (energy < maxEnergy)
        {
            energy += energyChange;
        }
    }

    

    public void SetIsActing()
    {
        isActing = true;
    }

    public void SetNotActing()
    {
        isActing = false;
    }

    public float DistanceFromFollower()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    private void OnEnable() {
        PlayerData.energyRecover += EnergyRecover;
    }

    private void OnDisable() {
        PlayerData.energyRecover -= EnergyRecover;
    }

    private void EnergyRecover()
    {
        energy += energyRegain;
        if (followState == FollowState.Following && energy > energyForAction)
        {
            ActivateAction(previousActionState);
        }
    }

    public ActionState GetActionState()
    {
        return actionState;
    }
}
