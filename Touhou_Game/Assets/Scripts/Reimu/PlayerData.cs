using System.Collections;
using System.Collections.Generic;
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
    public Vector2 bombDistance = new Vector2(2,0);
    public KeyCode bombButton = KeyCode.Q;
    public float bombRotationSpeed = 1;
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

        StartCoroutine(BombTargeting());
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

    private IEnumerator BombTargeting()
    {
        float startTime = Time.time;

        Vector3 rightBombPosition = transform.position + transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);
        Vector3 leftBombPosition = transform.position - transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);

        GameObject bomb1 = Instantiate(bombPrefab, rightBombPosition, Quaternion.identity);
        GameObject bomb2 = Instantiate(bombPrefab, leftBombPosition, Quaternion.identity);

        while (Time.time < startTime + invulnerableTime)
        {
            bomb1.transform.position = transform.position + transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);
            bomb2.transform.position = transform.position - transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);

            bomb1.transform.RotateAround(bomb1.transform.position, new Vector3(0,0,1), bombRotationSpeed);
            bomb2.transform.RotateAround(bomb2.transform.position, new Vector3(0,0,1), bombRotationSpeed);

            yield return null;
        }
        List<GameObject> gos = new List<GameObject>();

        gos.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        if (gos != null){
            GameObject closest1, closest2;

            closest1 = FindClosestEnemy(bomb1.transform.position, gos);
            Debug.Log(closest1);
            gos.Remove(closest1);

            if (gos.Count > 0)
            {
                closest2 = FindClosestEnemy(bomb2.transform.position, gos);
            } else {
                closest2 = closest1;
            }
        
            bomb1.GetComponent<BombController>().Target(closest1);
            bomb2.GetComponent<BombController>().Target(closest2);

        } else {
            Destroy(bomb1);
            Destroy(bomb2);
        }
    }

    private GameObject FindClosestEnemy(Vector3 bombPosition, List<GameObject> gos)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - bombPosition;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }
}