using UnityEngine;

public class BackAndForthPatrol : MonoBehaviour {
    public float patrolSpeed = 5f; // How fast the enemy moves along the path
    public float patrolDistance = 5f; // How far the enemy moves before going back

    public enum Direction
    {
        Horizontal,
        Vertical,
        DiagonalUpRight,
        DiagonalUpLeft,
    }

    public Direction direction = Direction.Horizontal;

    public float currentPatrolDistance = 0; // How far the enemy has currently patrolled
    private int directionFactor = 1; // Whether the enemy is currently moving forwards (1) or backwards (-1)

    private void Update() {
        Vector3 moveVector = Vector3.zero;

        switch (direction)
        {
            case Direction.Horizontal:
                moveVector = new Vector3(patrolSpeed * directionFactor, 0, 0);
                break;
            case Direction.Vertical:
                moveVector = new Vector3(0, patrolSpeed * directionFactor, 0);
                break;
            case Direction.DiagonalUpRight:
                moveVector = new Vector3(patrolSpeed * directionFactor, patrolSpeed * directionFactor, 0);
                break;
            case Direction.DiagonalUpLeft:
                moveVector = new Vector3(-patrolSpeed * directionFactor, patrolSpeed * directionFactor, 0);
                break;
        }

        transform.position += moveVector * Time.deltaTime;
        currentPatrolDistance += patrolSpeed * Time.deltaTime;

        if (currentPatrolDistance >= patrolDistance)
        {
            directionFactor *= -1; // Reverse direction
            currentPatrolDistance = 0f; // Reset patrol distance
        }
    }
}