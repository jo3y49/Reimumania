using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f; // Speed of character movement
    public float sprint = 1.8f; // Speed multipler for sprint
    public KeyCode sprintButton = KeyCode.LeftShift;
    private Vector2 moveDirection;
    public PlayerData.Direction direction;

    private void Update() {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Determine the direction
        if (!Mathf.Approximately(moveX, 0) || !Mathf.Approximately(moveY, 0)) // If there is movement
        {
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY)) // If horizontal movement is greater than vertical
            {
                if (moveX > 0) 
                    direction = PlayerData.Direction.Right;
                else 
                    direction = PlayerData.Direction.Left;
            }
            else // Vertical movement is greater
            {
                if (moveY > 0)
                    direction = PlayerData.Direction.Up;
                else 
                    direction = PlayerData.Direction.Down;
            }
        }

        // Apply the movement
        if (Input.GetKey(sprintButton))
        {
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speed * sprint * Time.deltaTime;
        } else {
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;
        }
    }
}