using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f; // Speed of character movement
    public float sprint = 1.8f; // Speed multipler for sprint
    public KeyCode sprintButton = KeyCode.LeftShift;
    private Vector2 moveDirection;
    public PlayerData.Direction direction = PlayerData.Direction.Up;

    private BoxCollider2D boxCollider; // Cache the box collider reference



    private void Awake() {
        // Retrieve the box collider
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        Movement();
    }   

    private void Movement()
    {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        float speedToUse = speed;
        if (Input.GetKey(sprintButton))
        {
            speedToUse *= sprint;
        }

        // Determine the direction
        DetermineDirection(moveX, moveY);

        Vector3 moveDirection3D = new Vector3(moveDirection.x, moveDirection.y, 0);

        float rayLength = speedToUse * Time.deltaTime;

        // Calculate the offset for the rays
        Vector2 extents = boxCollider.bounds.extents;
        Vector2 middleLeftOffset = new Vector2(-extents.x, 0);
        Vector2 middleRightOffset = new Vector2(extents.x, 0);
        Vector2 topLeftOffset = new Vector2(-extents.x, extents.y);
        Vector2 topMiddleOffset = new Vector2(0, extents.y);
        Vector2 topRightOffset = new Vector2(extents.x, extents.y);
        Vector2 bottomLeftOffset = new Vector2(-extents.x, -extents.y);
        Vector2 bottomMiddleOffset = new Vector2(0, -extents.y);
        Vector2 bottomRightOffset = new Vector2(extents.x, -extents.y);

        Vector2 ray1Start = (Vector2)transform.position;
        Vector2 ray2Start = (Vector2)transform.position;
        Vector2 ray3Start = (Vector2)transform.position;
        Vector2 ray4Start = (Vector2)transform.position;
        Vector2 ray5Start = (Vector2)transform.position;
        Vector2 ray6Start = (Vector2)transform.position;

        if (moveDirection.x == 0 || moveDirection.y == 0)
        {
            ray1Start += moveDirection * extents.magnitude/10;
            ray2Start += moveDirection * extents.magnitude/10;
            ray3Start += moveDirection * extents.magnitude/10;

            if (moveDirection.x > 0) {
                ray1Start += middleRightOffset;
                ray2Start += bottomRightOffset;
                ray3Start += topRightOffset;
            } else if (moveDirection.x < 0) {
                ray1Start += middleLeftOffset;
                ray2Start += bottomLeftOffset;
                ray3Start += topLeftOffset;
            }

            if (moveDirection.y > 0) {
                ray1Start += topMiddleOffset;
                ray2Start += topLeftOffset;
                ray3Start += topRightOffset;
            } else if (moveDirection.y < 0) {
                ray1Start += bottomMiddleOffset;
                ray2Start += bottomLeftOffset;
                ray3Start += bottomRightOffset;
            }
        } else 
        {
            ray1Start += new Vector2(moveDirection.x, 0) * extents.magnitude/10;
            ray2Start += new Vector2(moveDirection.x, 0) * extents.magnitude/10;
            ray3Start += new Vector2(moveDirection.x, 0) * extents.magnitude/10;
            ray4Start += new Vector2(0, moveDirection.y) * extents.magnitude/10;
            ray5Start += new Vector2(0, moveDirection.y) * extents.magnitude/10;
            ray6Start += new Vector2(0, moveDirection.y) * extents.magnitude/10;

            if (moveDirection.x > 0) {
                ray1Start += middleRightOffset;
                ray2Start += bottomRightOffset;
                ray3Start += topRightOffset;
            } else if (moveDirection.x < 0) {
                ray1Start += middleLeftOffset;
                ray2Start += bottomLeftOffset;
                ray3Start += topLeftOffset;
            }

            if (moveDirection.y > 0) {
                ray4Start += topMiddleOffset;
                ray5Start += topLeftOffset;
                ray6Start += topRightOffset;
            } else if (moveDirection.y < 0) {
                ray4Start += bottomMiddleOffset;
                ray5Start += bottomLeftOffset;
                ray6Start += bottomRightOffset;
            }
        }

        if (moveDirection.x == 0 || moveDirection.y == 0)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(ray1Start, moveDirection, rayLength);
            RaycastHit2D hit2 = Physics2D.Raycast(ray2Start, moveDirection, rayLength);
            RaycastHit2D hit3 = Physics2D.Raycast(ray3Start, moveDirection, rayLength);

            if ((hit1.collider != null && hit1.collider.CompareTag("Environment")) ||
                (hit2.collider != null && hit2.collider.CompareTag("Environment")) ||
                (hit3.collider != null && hit3.collider.CompareTag("Environment")))
            {
                return;
            }
        } 
        else 
        {
            RaycastHit2D hit1 = Physics2D.Raycast(ray1Start, new Vector2(moveDirection.x, 0), rayLength);
            RaycastHit2D hit2 = Physics2D.Raycast(ray2Start, new Vector2(moveDirection.x, 0), rayLength);
            RaycastHit2D hit3 = Physics2D.Raycast(ray3Start, new Vector2(moveDirection.x, 0), rayLength);
            RaycastHit2D hit4 = Physics2D.Raycast(ray4Start, new Vector2(0, moveDirection.y), rayLength);
            RaycastHit2D hit5 = Physics2D.Raycast(ray5Start, new Vector2(0, moveDirection.y), rayLength);
            RaycastHit2D hit6 = Physics2D.Raycast(ray6Start, new Vector2(0, moveDirection.y), rayLength);

            if ((hit1.collider != null && hit1.collider.CompareTag("Environment")) ||
                (hit2.collider != null && hit2.collider.CompareTag("Environment")) ||
                (hit3.collider != null && hit3.collider.CompareTag("Environment")) ||
                (hit4.collider != null && hit4.collider.CompareTag("Environment")) ||
                (hit5.collider != null && hit5.collider.CompareTag("Environment")) ||
                (hit6.collider != null && hit6.collider.CompareTag("Environment")))
            {
                return;
            }
        }

        // Otherwise, move the player
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * speedToUse * Time.deltaTime;
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
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}