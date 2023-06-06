using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour {

    public float speed = 10f;
    public float damage = 100f;
    public void Target(GameObject enemy)
    {
        StartCoroutine(SeekEnemy(enemy.transform));
    }
    public void Target(Vector3 direction)
    {
        StartCoroutine(ShootStraight(direction));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyData>().Shot(damage);
        }
    }

    private IEnumerator SeekEnemy(Transform enemy)
    {
        Vector3 direction = Vector3.zero;

        while (enemy != null && Vector3.Distance(transform.position, enemy.position) > 0.01f)
        {
            direction = (enemy.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ShootStraight(direction));
        
    }
    private IEnumerator ShootStraight(Vector3 direction)
    {
        float timeSpentMovingWithoutTarget = 0f;
        while(timeSpentMovingWithoutTarget < 5f)
        {
            transform.position += direction * speed * Time.deltaTime;
            timeSpentMovingWithoutTarget += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}