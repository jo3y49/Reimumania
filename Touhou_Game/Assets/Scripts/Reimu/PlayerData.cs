using System;
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
    public float bombRange = 10f;
    public KeyCode bombButton = KeyCode.Q;
    public KeyCode combatToggle = KeyCode.E;

    private bool isAlive = true;
    public bool isHittable = true;
    
    private PlayerShooting shootScript;
    private PlayerMovement moveScript;
    private Collider2D objectCollector;
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
    public Direction direction = Direction.Up;

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
    public static Action energyRecover;
    private Renderer hitboxRenderer;
    private GameDataManager gameData;

    private void Awake() {
        shootScript = GetComponent<PlayerShooting>();
        moveScript = GetComponent<PlayerMovement>();
        objectCollector = transform.GetChild(1).gameObject.GetComponent<Collider2D>();
        playerRenderer = GetComponent<Renderer>();
        hitboxRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    private void Start() {
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameData = gameManager.GetComponent<GameDataManager>();
        gameManager.GetComponent<PersistenceManager>().AddPersistentObject(gameObject);

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
                if (Input.GetKeyDown(bombButton) && bombs > 0 && invulnerableCoroutine == null)
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
        if (isHittable)
        {
            if (lives > 0)
            {
                StartCoroutine(Respawn());
            } else {
                gameData.GameOver();
            }
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
    public void CollectLife(GameObject life)
    {
        Destroy(life);
        lives++;
        gameData.AddLives();
    }
    public void CollectBomb(GameObject bomb)
    {
        Destroy(bomb);
        bombs++;
        gameData.AddBombs();
    }
    public void CollectEnergy(GameObject energy)
    {
        Destroy(energy);
        energyRecover?.Invoke();
    }

    private void Bomb()
    {
        bombs--;
        gameData.LoseBombs();
        isHittable = false;

        invulnerableCoroutine = StartCoroutine(Invulnerable());

        StartCoroutine(BombSpawning());
    }

    private IEnumerator Respawn()
    {
        hitboxRenderer.gameObject.SetActive(false);
        isAlive = isHittable = moveScript.enabled = shootScript.enabled = playerRenderer.enabled = objectCollector.enabled = false;
        
        lives -= 1;
        gameData.LoseLives();

        yield return new WaitForSeconds(respawnTime);

        hitboxRenderer.gameObject.SetActive(true);
        isAlive = moveScript.enabled = playerRenderer.enabled = objectCollector.enabled = true;
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
        invulnerableCoroutine = null;
    }

    private IEnumerator BombSpawning()
    {
        float startTime = Time.time;

        Vector3 rightBombPosition = transform.position + transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);
        Vector3 leftBombPosition = transform.position - transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);

        GameObject bomb1 = Instantiate(bombPrefab, rightBombPosition, Quaternion.identity);
        GameObject bomb2 = Instantiate(bombPrefab, leftBombPosition, Quaternion.identity);

        BombController bombController1 = bomb1.GetComponent<BombController>();
        BombController bombController2 = bomb2.GetComponent<BombController>();

        StartCoroutine(bombController1.StartRotation());
        StartCoroutine(bombController2.StartRotation());

        while (Time.time < startTime + invulnerableTime * .75f)
        {
            bomb1.transform.position = transform.position + transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);
            bomb2.transform.position = transform.position - transform.right * bombDistance.x + new Vector3(0, bombDistance.y, 0);

            yield return null;
        }

        BombTargeting(bomb1, bomb2, bombController1, bombController2);
    }

    private void BombTargeting(GameObject bomb1, GameObject bomb2, BombController bombController1, BombController bombController2)
    {
        List<Transform> gos = new List<Transform>();

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= bombRange)
            {
                gos.Add(enemy.transform);
            }
        }

        if (gos.Count > 0){
            Transform closest1, closest2;

            closest1 = FindClosestEnemy(bomb1.transform.position, gos);
            gos.Remove(closest1);

            if (gos.Count > 0)
            {
                closest2 = FindClosestEnemy(bomb2.transform.position, gos);
            } else {
                closest2 = closest1;
            }
        
            bombController1.Target(closest1);
            bombController2.Target(closest2);

        } else {
            bombController1.Target(transform.up);
            bombController2.Target(transform.up);
        }
    }

    private Transform FindClosestEnemy(Vector3 bombPosition, List<Transform> gos)
    {
        Transform closest = null;
        float distance = Mathf.Infinity;

        foreach (Transform go in gos)
        {
            Vector3 diff = go.position - bombPosition;
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