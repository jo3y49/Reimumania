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
    public Animator aMouth;
    public Vector2 bombDistance = new Vector2(2,0);
    public float bombRange = 10f;
    public KeyCode bombButton = KeyCode.Q;
    public KeyCode combatToggle = KeyCode.E;

    private bool isAlive = true;
    public bool isHittable = true;
    public bool bossFight = false;
    
    private PlayerShooting shootScript;
    private PlayerMovement moveScript;
    private Collider2D objectCollector;
    private GameObject playerBody;
    private Rigidbody2D rb;
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
        DownLeft
    }
    public Direction direction = Direction.Up;

    public enum Upgrade
    {
        L1,
        L2,
        L3,
    }

    public Upgrade upgrade = Upgrade.L1;

    public static Action energyRecover;
    private Collider2D hitboxCollider;
    private GameDataManager gameData;

    private void Awake() {
        shootScript = GetComponent<PlayerShooting>();
        moveScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        objectCollector = transform.GetChild(1).gameObject.GetComponent<Collider2D>();
        playerBody = transform.GetChild(2).gameObject;
        hitboxCollider = transform.GetChild(0).gameObject.GetComponent<Collider2D>();
    }

    private void Start() {
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameData = gameManager.GetComponent<GameDataManager>();

        gameData.GetSavedPlayerData(this);
        aMouth.SetInteger("Lives", lives);
            
    }

    private void Update() {

        if (isAlive)
        {
            direction = shootScript.direction;
            if (Input.GetKeyDown(bombButton) && bombs > 0 && invulnerableCoroutine == null)
            {
                Bomb();
            }
                
        }

    }

    public void Shot(float bulletDamage)
    {
        if (isHittable)
        {
            if (lives > 1)
            {
                StartCoroutine(Respawn());
            } else {
                gameData.GameOver();
            }
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
        if (lives < 3)
        {
            Destroy(life);
            lives++;
            gameData.AddLives();
            aMouth.SetInteger("Lives", lives);
        }
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
        rb.velocity = Vector2.zero;
        lives -= 1;
        gameData.LoseLives();
        
        hitboxCollider.gameObject.SetActive(false);
        playerBody.SetActive(false);
        isAlive = isHittable = moveScript.enabled = shootScript.enabled = objectCollector.enabled = false;
        
        yield return new WaitForSeconds(respawnTime);

        hitboxCollider.gameObject.SetActive(true);
        playerBody.SetActive(true);
        isAlive = moveScript.enabled = objectCollector.enabled = shootScript.enabled = true;

        aMouth.SetInteger("Lives", lives);

        StartCoroutine(Invulnerable());
    }
    private IEnumerator Invulnerable()
    {
        float startTime = Time.time;
        Material[] material = new Material[4];

        Color[] originalColor = new Color[4];

        for (int i = 0; i < 4; i++)
        {
            material[i] = playerBody.transform.GetChild(i).gameObject.GetComponent<Renderer>().material;
            originalColor[i] = material[i].color;
        }

        while (Time.time < startTime + invulnerableTime)
        {
            // interpolate between the original color and the flash color based on a sine wave
            float t = Mathf.Sin(Time.time * flashSpeed) * 0.5f + 0.5f;
            for (int i = 0; i < 4; i++)
            {
                material[i].color = new Color(originalColor[i].r, originalColor[i].g, originalColor[i].b, Mathf.Lerp(0, 1, t));
            }
            

            yield return null;
        }

        for (int i = 0; i < 4; i++)
        {
            material[i].color = originalColor[i];
        }
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

        if (!bossFight)
            BombTargeting(bomb1, bomb2, bombController1, bombController2);
        else
        {
            Transform bossPosition = GameObject.FindGameObjectWithTag("Boss").transform;
            bombController1.Target(bossPosition);
            bombController2.Target(bossPosition);
        }
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