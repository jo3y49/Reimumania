using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f; // Speed of character movement
    public float sprint = 1.8f; // Speed multipler for sprint
    public KeyCode sprintButton = KeyCode.LeftShift;
    public PlayerData.Direction direction = PlayerData.Direction.Up;



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
        if (!Mathf.Approximately(moveX, 0) || !Mathf.Approximately(moveY, 0)) 
        {
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
            {
                if (moveX > 0){
                    direction = PlayerData.Direction.Right;
                }
                else {
                    direction = PlayerData.Direction.Left;
                }
            }
            else
            {
                if (moveY > 0){
                    direction = PlayerData.Direction.Up;
                }
                else {
                    direction = PlayerData.Direction.Down;
                }
            }
        }
    }
}