using UnityEngine;

public class BossData : MonoBehaviour, Shootable
{
    public int lives = 3;
    public float health = 50f;
    public float shield1 = 100f;
    public float shield2 = 100f;
    public bool isShooting = true;
    private float maxHealth, maxShield1, maxShield2;
    [SerializeField] private BossPattern[] bossPatterns;
    private BossPattern activePattern;
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

        foreach (BossPattern b in bossPatterns)
        {
            b.Disable();
        }

        EnablePattern(bossPatterns[0]);
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