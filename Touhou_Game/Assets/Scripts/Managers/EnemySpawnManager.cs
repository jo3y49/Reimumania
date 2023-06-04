using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public Vector2[] spawnLocation;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject enemy = getBaseEnemy(spawnLocation[0]);
            enemy.AddComponent<BackAndForthPatrol>();
            enemy.AddComponent<ShootForward>();
            enemy.GetComponent<Renderer>().material.color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject enemy = getBaseEnemy(spawnLocation[1]);
            enemy.AddComponent<CirclePatrol>();
            enemy.AddComponent<ShootAtPlayer>();
            enemy.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private GameObject getBaseEnemy(Vector2 spawnLocation)
    {
        return Instantiate(enemyPrefab, spawnLocation, transform.rotation);
    }
}