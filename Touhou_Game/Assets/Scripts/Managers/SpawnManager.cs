using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject enemyPrefab, bombPrefab, lifePrefab, energyPrefab;
    public Vector2[] spawnLocation;
    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject enemy = getBaseEnemy(spawnLocation[4]);
            enemy.AddComponent<BackAndForthPatrol>();
            BackAndForthPatrol patrol = enemy.GetComponent<BackAndForthPatrol>();
            patrol.patrolDistance = 15;
            patrol.patrolSpeed = 10f;
            enemy.AddComponent<ShootAtPlayer>();
            enemy.GetComponent<Renderer>().material.color = Color.magenta;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameObject enemy = getBaseEnemy(spawnLocation[5]);
            enemy.AddComponent<BackAndForthPatrol>();
            BackAndForthPatrol patrol = enemy.GetComponent<BackAndForthPatrol>();
            patrol.patrolDistance = 15;
            patrol.patrolSpeed = 10f;
            enemy.AddComponent<ShootAtPlayer>();
            enemy.GetComponent<Renderer>().material.color = Color.magenta;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(bombPrefab, spawnLocation[3] + (Vector2)player.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Instantiate(lifePrefab, spawnLocation[3] + (Vector2)player.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Instantiate(energyPrefab, spawnLocation[3] + (Vector2)player.transform.position, Quaternion.identity);
        }
    }

    private GameObject getBaseEnemy(Vector2 spawnLocation)
    {
        return Instantiate(enemyPrefab, spawnLocation, transform.rotation);
    }
}