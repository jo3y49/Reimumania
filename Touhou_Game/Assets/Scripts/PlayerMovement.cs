using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f; // Speed of character movement
    public float sprint = 1.8f; // Speed multipler for sprint
    private Vector2 moveDirection;

    private void Update() {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Apply the movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speed * sprint * Time.deltaTime;
        } else {
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;
        }
    }

    
    
}