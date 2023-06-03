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

    private GameDataManager gameData;

    private void Start() {
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameData = gameManager.GetComponent<GameDataManager>();
        gameManager.GetComponent<PersistenceManager>().AddPersistentObject(gameObject);

        shootScript = GetComponent<PlayerShooting>();
        moveScript = GetComponent<PlayerMovement>();
        playerRenderer = GetComponent<Renderer>();

        gameData.getSavedPlayerData(this);

        if (state == State.Combat)
            shootScript.enabled = true;
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
                if (state == State.Combat)
                {
                    state = State.Default;
                    moveScript.direction = shootScript.direction;
                    
                    shootScript.enabled = false;
                } else 
                {
                    state = State.Combat;
                    shootScript.direction = moveScript.direction;
                    
                    shootScript.enabled = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (upgrade == Upgrade.L1)
                upgrade = Upgrade.L2;
            else
                upgrade = Upgrade.L1;

            gameData.setUpgrade(upgrade);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            coins++;
            gameData.addCoins();
        }
    }

    private IEnumerator Bomb()
    {
        bombs--;
        
        yield return null;
    }

    public void Shot(float bulletDamage)
    {
        if (lives > 0 && isAlive)
        {
            StartCoroutine(Respawn());
            Debug.Log("shot");
        }
    }

    private IEnumerator Respawn()
    {
        Renderer hitboxRenderer = transform.GetChild(0).GetComponent<Renderer>();

        isAlive = moveScript.enabled = shootScript.enabled = playerRenderer.enabled = hitboxRenderer.enabled = false;
        
        lives--;

        yield return new WaitForSeconds(respawnTime);

        if (state == State.Combat)
            shootScript.enabled = true;

        isAlive = moveScript.enabled = playerRenderer.enabled = hitboxRenderer.enabled = true;
        
    }
}