using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowerAttack : MonoBehaviour, FollowerAction {
    public FollowerController followerController;
    public float searchTime = 3f;
    public float attackSpeed = 2f;
    public float attackRange = 5f;
    public float attackCooldown = 5f;
    public float distanceToRestart = 1f;
    public int energyDrain = 20;
    private bool cancel = false;
    private List<GameObject> enemies;
    private Coroutine huntCoroutine, lockOnCoroutine, attackCoroutine;

    private void Awake() {
        followerController = GetComponent<FollowerController>();
    }

    public void Activate() {
        if (huntCoroutine != null) 
        {
            StopCoroutine(huntCoroutine);
        }
        cancel = false;
        huntCoroutine = StartCoroutine(Hunt());
        
    }

    public void Deactivate() {

        cancel = true;

        if (huntCoroutine != null) 
        {
            StopCoroutine(huntCoroutine);
            huntCoroutine = null;
        }
        if (lockOnCoroutine != null) 
        {
            StopCoroutine(lockOnCoroutine);
            lockOnCoroutine = null;
        }
        if (attackCoroutine != null) 
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    private IEnumerator Hunt()
    {
        while (true)
        {
            Debug.Log("hunting");
            yield return new WaitForSeconds(attackCooldown);

            if (FindEnemies())
            {
                yield return StartCoroutine(LockOn());
            }
        }
    }

    private IEnumerator LockOn()
    {
        Debug.Log("lockon");
        float timeSearched = 0;
        while (timeSearched < searchTime)
        {
            if (cancel)
            {
                cancel = false;
                yield break;
            }
            timeSearched += Time.deltaTime;
            yield return null;
        }

        if (FindEnemies())
        {
            followerController.SetIsActing();
            yield return StartCoroutine(Attack());
        } else {
            yield return StartCoroutine(Hunt());
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log("attack");
        
        GameObject enemy = FindTarget();

        Vector3 direction = Vector3.zero;

        while (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) > 0.4f)
        {
            direction = (enemy.transform.position - transform.position).normalized;
            transform.position += direction * attackSpeed * Time.deltaTime;

            if (enemy == null)
            {
                break;
            }
            yield return null;
        }

        if (enemy != null)
        {
            enemy.GetComponent<EnemyData>().Shot(100f);
        }

        followerController.SetNotActing();

        followerController.energy -= energyDrain;

        Activate();
    }

    private bool FindEnemies()
    {
        List<GameObject> gos = new List<GameObject>();

        foreach(GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance <= attackRange)
            {
                gos.Add(e);
            }
        }

        enemies = gos;

        return enemies.Count > 0;
    }

    private GameObject FindTarget()
    {
        GameObject enemy = enemies[0];
        float enemyDistance = attackRange;

        foreach(GameObject e in enemies)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < enemyDistance)
            {
                enemy = e;
                enemyDistance = distance;
            }
        }

        return enemy;
    }
}