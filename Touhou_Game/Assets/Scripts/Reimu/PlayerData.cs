using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int lives = 3;
    public int bombs = 3;
    
    public int coins = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            coins++;
        }
    }
}