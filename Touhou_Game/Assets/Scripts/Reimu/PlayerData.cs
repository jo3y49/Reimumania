using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int lives = 3;
    public int bombs = 3;
    public int coins = 0;
    public KeyCode bombButton = KeyCode.Q;
    public KeyCode combatToggle = KeyCode.E;
    
    private PlayerShooting shootScript;
    private PlayerMovement moveScript;
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

        shootScript = gameObject.GetComponent<PlayerShooting>();
        moveScript = gameObject.GetComponent<PlayerMovement>();

        gameData.getSavedPlayerData(this);

        if (state == State.Combat)
            shootScript.enabled = true;
    }

    private void Update() {

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
        
        if (Input.GetKeyDown(KeyCode.L)){
            if (upgrade == Upgrade.L1)
                upgrade = Upgrade.L2;
            else
                upgrade = Upgrade.L1;

            gameData.setUpgrade(upgrade);
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
}