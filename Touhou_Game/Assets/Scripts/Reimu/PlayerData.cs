using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour, Shootable
{
    public int lives = 3;
    public int bombs = 3;
    public int coins = 0;
    public float respawnTime = 2;
    public float invulnerableTime = 2;
    public float flashSpeed = 5f;   
    public GameObject bombPrefab;
    public KeyCode bombButton = KeyCode.Q;
    public KeyCode combatToggle = KeyCode.E;

    private bool isAlive = true;
    public bool isHittable = true;
    
    private PlayerShooting shootScript;
    private PlayerMovement moveScript;
    private Renderer playerRenderer;
    private Coroutine invulnerableCoroutine;
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
    private Renderer hitboxRenderer;
    private GameDataManager gameData;

    private void Start() {
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameData = gameManager.GetComponent<GameDataManager>();
        gameManager.GetComponent<PersistenceManager>().AddPersistentObject(gameObject);

        shootScript = GetComponent<PlayerShooting>();
        moveScript = GetComponent<PlayerMovement>();
        playerRenderer = GetComponent<Renderer>();
        hitboxRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();

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
                    Bomb();
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
        if (lives > 0 && isHittable)
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
                hitboxRenderer.enabled = false;
                shootScript.enabled = false;

            } else 
            {
                state = State.Combat;
                shootScript.SetDirection(direction);
                hitboxRenderer.enabled = true;
                shootScript.enabled = true;
            }
    }

    public void CollectCoin(GameObject coin)
    {
        Destroy(coin);
        coins++;
        gameData.AddCoins();
    }

    private void Bomb()
    {
        bombs--;
        isHittable = false;
        if (invulnerableCoroutine != null)
            StopCoroutine(invulnerableCoroutine);
        invulnerableCoroutine = StartCoroutine(Invulnerable());
    }

    private IEnumerator Respawn()
    {
        hitboxRenderer.gameObject.SetActive(false);
        isAlive = isHittable = moveScript.enabled = shootScript.enabled = playerRenderer.enabled = false;
        
        lives -= 1;

        yield return new WaitForSeconds(respawnTime);

        hitboxRenderer.gameObject.SetActive(true);
        isAlive = moveScript.enabled = playerRenderer.enabled = true;
        if (state == State.Combat)
            shootScript.enabled = true;

        StartCoroutine(Invulnerable());
    }
    private IEnumerator Invulnerable()
    {
        float startTime = Time.time;
        Material material = playerRenderer.material;
        Color originalColor = material.color;

        while (Time.time < startTime + invulnerableTime)
        {
            // interpolate between the original color and the flash color based on a sine wave
            float t = Mathf.Sin(Time.time * flashSpeed) * 0.5f + 0.5f;
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, t));

            yield return null;
        }

        material.color = originalColor;
        isHittable = true;
    }
}