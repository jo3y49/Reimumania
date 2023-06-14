using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f; // Speed of character movement
    public float sprint = 1.8f; // Speed multipler for sprint
    public KeyCode sprintButton = KeyCode.LeftShift;
    private Vector2 moveDirection;
    private Vector3 nextPosition;
    public PlayerData.Direction direction = PlayerData.Direction.Up;

    private bool hasCollided = false;



    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        float speedToUse = speed;
        if (Input.GetKey(sprintButton))
        {
            speedToUse *= sprint;
        }

        DetermineDirection(moveX, moveY);
        rb2d.velocity = moveDirection * speedToUse;
    }

    private void DetermineDirection(float moveX, float moveY) {
        if (!Mathf.Approximately(moveX, 0) || !Mathf.Approximately(moveY, 0)) // If there is movement
        {
            float angle = 0;
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY)) // If horizontal movement is greater than vertical
            {
                if (moveX > 0){
                    direction = PlayerData.Direction.Right;
                    angle = 270;
                }
                else {
                    direction = PlayerData.Direction.Left;
                    angle = 90;
                }
            }
            else // Vertical movement is greater
            {
                if (moveY > 0){
                    direction = PlayerData.Direction.Up;
                }
                else {
                    direction = PlayerData.Direction.Down;
                    angle = 180;
                }
            }
            // transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}