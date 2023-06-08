using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject enemyPrefab, bombPrefab;
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
            enemy.GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject enemy = getBaseEnemy(spawnLocation[2]);
            enemy.AddComponent<CirclePatrol>();
            enemy.AddComponent<ShootAtPlayer>();
            enemy.GetComponent<CirclePatrol>().clockwise = false;
            enemy.GetComponent<Renderer>().material.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject enemy = getBaseEnemy(spawnLocation[0]);
            enemy.AddComponent<BackAndForthPatrol>();
            enemy.AddComponent<ShootAtPlayer>();
            enemy.GetComponent<Renderer>().material.color = Color.black;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(bombPrefab, spawnLocation[3], Quaternion.identity);
        }
    }

    private GameObject getBaseEnemy(Vector2 spawnLocation)
    {
        return Instantiate(enemyPrefab, spawnLocation, transform.rotation);
    }
}