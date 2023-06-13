using UnityEngine;

public class BossData : MonoBehaviour, Shootable
{
    public int lives = 3;
    public float health = 50f;
    public float shield1 = 100f;
    public float shield2 = 100f;
    public bool isShooting = true;
    public float arenaWidth, arenaHeight, leftLocation, rightLocation, topLocation, bottomLocation;
    private float maxHealth, maxShield1, maxShield2;
    public GameObject bulletPrefab;
    public GameObject portalPrefab;
    public Transform playerLocation;
    [SerializeField] private BossPattern[] bossPatterns;
    [SerializeField] private BossMovement[] bossMovements;
    private BossPattern activePattern;
    private BossMovement activeMovement;
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

        // foreach (BossPattern b in bossPatterns)
        // {
        //     b.Disable();
        // }

        // EnablePattern(bossPatterns[0]);
    }

    private void NextLife()
    { 
        health = maxHealth;
        shield1 = maxShield1;
        shield2 = maxShield2;
        state = State.Healthy;
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
            }
            break;
            case State.Damaged:
            shield2 -= bulletDamage;
            if (shield2 <= 0)
            {
                state = State.Stunned;
                shield2 = 0;
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
                } else 
                {
                    Destroy(gameObject);
                }
            }
            break;
        }
        Debug.Log(shield1 + " " + shield2 + " " + health);
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
}