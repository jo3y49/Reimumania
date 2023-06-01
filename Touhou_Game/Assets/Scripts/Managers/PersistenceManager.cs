using UnityEngine;
using System.Collections.Generic;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager instance;

    [SerializeField] private List<GameObject> persistentObjects = new List<GameObject>();

    private void Start() {
        foreach (GameObject obj in persistentObjects)
        {
            DontDestroyOnLoad(obj);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
