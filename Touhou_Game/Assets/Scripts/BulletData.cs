using UnityEngine;

public class BulletData : MonoBehaviour
{
    public static BulletData Instance { get; private set; } // Singleton instance

    public float bulletSpeed = 10f; // Shared bullet speed
    public float fireRate = 5f; // Shared fire rate

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
