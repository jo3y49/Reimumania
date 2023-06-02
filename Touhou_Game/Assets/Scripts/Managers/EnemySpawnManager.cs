using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public Vector2 spawnLocation;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject enemy = getBaseEnemy();
            enemy.AddComponent<BackAndForthPatrol>();
            enemy.AddComponent<ShootForward>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject enemy = getBaseEnemy();
            enemy.AddComponent<CirclePatrol>();
            enemy.AddComponent<ShootAtPlayer>();
        }
    }

    private GameObject getBaseEnemy()
    {
        return Instantiate(enemyPrefab, spawnLocation, transform.rotation);
    }
}