using UnityEngine;

public class CirclePatrol : MonoBehaviour {
    public float patrolRadius = 1f; // The radius of the circular path
    public float patrolSpeed = 1f; // How fast the enemy moves along the path

    public bool clockwise = true;

    private float patrolAngle = 0f; // The current angle along the circular path

    private void Start() {
        if (clockwise)
            patrolSpeed = -patrolSpeed;
    }

    private void Update() {
        if (Time.timeScale == 0f) {
            return;
        }
        // Move along the circular patrol path
        patrolAngle += patrolSpeed * Time.deltaTime;
        Vector3 patrolOffset = new Vector3(Mathf.Cos(patrolAngle), Mathf.Sin(patrolAngle), 0) * patrolRadius;
        transform.position += patrolOffset / 500f;
    }

}