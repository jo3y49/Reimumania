using UnityEngine;
using System.Collections;

public class BossData : MonoBehaviour, Shootable
{
    public int lives = 3;
    public float health = 50f;
    public float shield1 = 100f;
    public float shield2 = 100f;
    public float respawnTime = 1f;
    public bool isShooting = true;
    public float arenaWidth, arenaHeight, leftLocation, rightLocation, topLocation, bottomLocation;
    private float maxHealth, maxShield1, maxShield2;
    public GameObject bulletPrefab;
    public GameObject portalPrefab;
    public Transform playerLocation;
    [SerializeField] private BossPattern[] bossPatterns;
    [SerializeField] private BossMovement[] bossMovements;
    [SerializeField] private PortalFollowPlayer portalFollowPlayer;
    private BossPattern activePattern;
    private BossMovement activeMovement;
    private Coroutine currentRoutine;
    public enum State
    {
        Healthy,
        Damaged,
        Stunned
    }

    public State state = State.Healthy;

    private void Awake() {
        maxHealth = health;
        maxShield1 = shield1;
        maxShield2 = shield2;
        leftLocation = -arenaWidth/2;
        rightLocation = arenaWidth/2;
        topLocation = arenaHeight/2;
        bottomLocation = -arenaHeight/2;
    }

    private void Start() {
        portalFollowPlayer.Disable();

        foreach (BossPattern b in bossPatterns)
        {
            b.Disable();
        }
        foreach (BossMovement b in bossMovements)
        {
            b.Disable();
        }

        EnablePattern(bossPatterns[1]);
        currentRoutine = StartCoroutine(NeutralWithCircle(5));
        EnableMovement(bossMovements[0]);
    }

    private void NextLife()
    { 
        
        health = maxHealth;
        shield1 = maxShield1;
        shield2 = maxShield2;
        state = State.Healthy;
        StartCoroutine(Respawn());
    }


    public void Shot(float bulletDamage)
    {
        switch (state)
        {
            case State.Healthy:
            shield1 -= bulletDamage;
            if (shield1 <= 0)
            {
                state = State.Damaged;
                shield1 = 0;
                DamagePhase();
            }
            break;
            case State.Damaged:
            shield2 -= bulletDamage;
            if (shield2 <= 0)
            {
                state = State.Stunned;
                shield2 = 0;
                StunnedPhase();
            }
            break;
            case State.Stunned:
            health -= bulletDamage;
            if (health <= 0)
            {
                health = 0;
                lives -= 1;
                if (lives > 0)
                {
                    NextLife();
                } else {
                    Destroy(gameObject);
                }
            }
            break;
        }
        
        Debug.Log(shield1 + " " + shield2 + " " + health);
    }

    private void HealthyPhase()
    {
        Reset();
        EnablePattern(bossPatterns[1]);
        currentRoutine = StartCoroutine(NeutralWithCircle(5));
        EnableMovement(bossMovements[0]);
        switch (lives)
        {
            case 2:
            bossPatterns[0].shootSpeed *= 1.1f;
            bossPatterns[1].shootSpeed *= 1.1f;
            bossMovements[0].moveSpeed *= 1.2f;
            break;
            case 1:
            portalFollowPlayer.Enable();
            break;
        }
    }

    private void DamagePhase()
    {
        Reset();
        EnableMovement(bossMovements[1]);
        EnablePattern(bossPatterns[1]);
        switch (lives)
        {
            case 2:
                activePattern.shootSpeed *= 1.2f;
                activeMovement.moveSpeed *= 1.5f;
            break;
            case 1:
                portalFollowPlayer.Enable();
            break;
        }
    }

    private void StunnedPhase()
    {
        Reset();
        switch (lives)
        {
            case 3:
            break;
            case 2:
            break;
            case 1:
            break;
        }
    }

    private IEnumerator NeutralWithCircle(float switchTime)
    {
        while (true)
        {
            if (activePattern == bossPatterns[0])
            {
                DisablePattern(activePattern);
                EnablePattern(bossPatterns[1]);
                    
            } else {
                DisablePattern(activePattern);
                EnablePattern(bossPatterns[0]);
            }

            yield return new WaitForSeconds(switchTime);
        }
    }

    private IEnumerator Respawn()
    {
        transform.position = new Vector3(0, topLocation + 5, transform.position.z);

        yield return new WaitForSeconds(respawnTime);

        HealthyPhase();
    }

    private void Reset()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Projectile")){
            Destroy(g);
        }

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
        if (activePattern != null)
            DisablePattern(activePattern);
        if (activeMovement != null)
            DisableMovement(activeMovement);

        portalFollowPlayer.Disable();
    }

    private void EnablePattern(BossPattern pattern)
    {
        pattern.Enable();
        activePattern = pattern;
    }
    private void DisablePattern(BossPattern pattern)
    {
        pattern.Disable();
        activePattern = null;
    }
    private void EnableMovement(BossMovement movement)
    {
        movement.Enable();
        activeMovement = movement;
    }
    private void DisableMovement(BossMovement movement)
    {
        movement.Disable();
        activeMovement = null;
    }
}