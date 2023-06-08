using UnityEngine;

public class PortalController : MonoBehaviour {
    public float distanceFromPortal = 1f;
    public PlayerData.Direction direction = PlayerData.Direction.Left;
    private Vector2 spawnLocation = Vector2.zero;

    private void Awake() {
        Vector2 directionVector = Vector2.zero;
        switch (direction)
        {
            case PlayerData.Direction.Left:
            directionVector = Vector2.left;
            break;
            case PlayerData.Direction.Right:
            directionVector = Vector2.right;
            break;
            case PlayerData.Direction.Up:
            directionVector = Vector2.up;
            break;
            case PlayerData.Direction.Down:
            directionVector = Vector2.down;
            break;
        }
        spawnLocation = transform.position + (Vector3)directionVector * distanceFromPortal;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) 
        {
            FindAnyObjectByType<PortalManager>().Teleport(spawnLocation);
        }
    }
}