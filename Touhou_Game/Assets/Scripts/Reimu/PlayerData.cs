using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour, Shootable
{
    public int lives = 3;
    public int bombs = 3;
    public int coins = 0;
    public float respawnTime = 2;
    public KeyCode bombButton = KeyCode.Q;
    public KeyCode combatToggle = KeyCode.E;

    public bool isAlive = true;
    
    private PlayerShooting shootScript;
    private PlayerMovement moveScript;
    private Renderer playerRenderer;
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
    public Direction direction = Direction.Down;

    public enum Upgrade
    {
        L1,
        L2,
        L3,
    }

    public Upgrade upgrade = Upgrade.L1;

    public enum State 
    {
        Default,
        Combat,
    }

    public State state = State.Default;
    private GameObject hitbox;
    private GameDataManager gameData;

    private void Start() {
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameData = gameManager.GetComponent<GameDataManager>();
        gameManager.GetComponent<PersistenceManager>().AddPersistentObject(gameObject);

        shootScript = GetComponent<PlayerShooting>();
        moveScript = GetComponent<PlayerMovement>();
        playerRenderer = GetComponent<Renderer>();
        hitbox = transform.GetChild(0).gameObject;

        gameData.GetSavedPlayerData(this);

        if (state == State.Combat){
            ToggleCombatState(State.Default);
        }
            
    }

    private void Update() {

        if (isAlive)
        {
            if (state == State.Combat)
            {
                direction = shootScript.direction;
                if (Input.GetKeyDown(bombButton) && bombs > 0)
                {
                    StartCoroutine(Bomb());
                }
                
            } else
            {
                direction = moveScript.direction;
            }

            if (Input.GetKeyDown(combatToggle))
            {
                ToggleCombatState(state);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (upgrade == Upgrade.L1)
                upgrade = Upgrade.L2;
            else
                upgrade = Upgrade.L1;

            gameData.SetUpgrade(upgrade);
        }
    }

    public void Shot(float bulletDamage)
    {
        if (lives > 0 && isAlive)
        {
            StartCoroutine(Respawn());
        }
    }

    private void ToggleCombatState(State oldState)
    {
        if (oldState == State.Combat)
            {
                state = State.Default;
                moveScript.direction = direction;
                hitbox.GetComponent<Renderer>().enabled = false;
                shootScript.enabled = false;

            } else 
            {
                state = State.Combat;
                shootScript.SetDirection(direction);
                hitbox.GetComponent<Renderer>().enabled = true;
                shootScript.enabled = true;
            }
    }

    public void CollectCoin(GameObject coin)
    {
        Destroy(coin);
        coins++;
        gameData.AddCoins();
    }

    private IEnumerator Bomb()
    {
        bombs--;
        
        yield return null;
    }

    private IEnumerator Respawn()
    {
        hitbox.SetActive(false);
        isAlive = moveScript.enabled = shootScript.enabled = playerRenderer.enabled = false;
        
        lives -= 1;

        yield return new WaitForSeconds(respawnTime);

        hitbox.SetActive(true);
        isAlive = moveScript.enabled = playerRenderer.enabled = true;
        if (state == State.Combat)
            shootScript.enabled = true;
    }
}