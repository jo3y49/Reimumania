using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int lives = 3;
    public int bombs = 3;
    public int coins = 0;
    public KeyCode bombButton = KeyCode.Q;
    public KeyCode combatToggle = KeyCode.E;
    
    private PlayerShooting shootScript;
    private PlayerMovement moveScript;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
    }
    public Direction direction = Direction.Down;

    public enum State 
    {
        Default,
        Combat,
    }

    public State state = State.Default;

    private void Start() {
        shootScript = gameObject.GetComponent<PlayerShooting>();
        moveScript = gameObject.GetComponent<PlayerMovement>();

        if (state == State.Combat)
            shootScript.enabled = true;
    }

    private void Update() {

        if (state == State.Combat)
        {
            direction = shootScript.direction;
            if (Input.GetKeyDown(bombButton) && bombs > 0)
            {
                StartCoroutine(Bomb());
            }
            
        } else 
        {
            direction = moveScript.direction;
        }

        if (Input.GetKeyDown(combatToggle))
        {
            if (state == State.Combat)
            {
                state = State.Default;
                moveScript.direction = shootScript.direction;
                
                shootScript.enabled = false;
            } else 
            {
                state = State.Combat;
                shootScript.direction = moveScript.direction;
                
                shootScript.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            coins++;
        }
    }

    private IEnumerator Bomb()
    {
        bombs--;
        Debug.Log(bombs);

        yield return null;
    }
}