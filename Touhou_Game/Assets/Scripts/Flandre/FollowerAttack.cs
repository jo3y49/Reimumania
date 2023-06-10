using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowerAttack : MonoBehaviour, FollowerAction {
    public FollowerController followerController;
    public float searchTime = 3f;
    public float attackSpeed = 2f;
    public float attackRange = 5f;
    public float attackCooldown = 5f;
    public float distanceToRestart = 2f;
    private List<GameObject> enemies;
    private Coroutine currentCoroutine;

    private void Awake() {
        followerController = GetComponent<FollowerController>();
    }

    public void Activate() {
        currentCoroutine = StartCoroutine(Hunt());
    }

    public void Deactivate() {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    private IEnumerator Hunt()
    {
        while (true)
        {
            if (FindEnemies())
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(LockOn());
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private IEnumerator LockOn()
    {
        float timeSearched = 0;
        while (timeSearched < searchTime)
        {
            timeSearched += Time.deltaTime;
            yield return null;
        }

        if (FindEnemies())
        {
            followerController.SetIsActing();
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Attack());
        } else {
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Hunt());
        }
    }

    private IEnumerator Attack()
    {
        GameObject enemy = FindTarget();

        Vector3 direction = Vector3.zero;

        while (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) > 0.1f)
        {
            direction = (enemy.transform.position - transform.position).normalized;
            transform.position += direction * attackSpeed * Time.deltaTime;
            yield return null;
        }

        enemy?.GetComponent<EnemyData>().Shot(100f);

        followerController.SetNotActing();

        StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(WaitForActivation());
    }

    public IEnumerator WaitForActivation()
    {
        while (distanceToRestart < followerController.DistanceFromFollower())
        {
            yield return new WaitForSeconds(1f);
        }

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