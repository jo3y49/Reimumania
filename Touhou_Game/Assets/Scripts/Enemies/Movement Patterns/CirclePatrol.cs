using UnityEngine;

public class CirclePatrol : MonoBehaviour {
    public float patrolRadius = 5f; // The radius of the circular path
    public float rotationSpeed = 1f;
    public float patrolSpeed = 3f; // How fast the enemy moves along the path

    public bool clockwise = true;

    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
        if (clockwise)
            patrolSpeed = -patrolSpeed;
    }

    private void Update() {
        if (Time.timeScale == 0f) {
            return;
        }
        float angle = Time.time * patrolSpeed;
        float x = Mathf.Cos(angle) * patrolRadius;
        float y = Mathf.Sin(angle) * patrolRadius;
        transform.position = startPosition + new Vector3(x, y, 0);
        transform.RotateAround(transform.position, new Vector3(0,0,1), rotationSpeed * Time.deltaTime * 100f); 
    }

}