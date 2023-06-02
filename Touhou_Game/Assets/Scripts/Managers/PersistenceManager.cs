using UnityEngine;
using System.Collections.Generic;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager instance;

    private List<GameObject> persistentObjects = new List<GameObject>();

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

    public void AddPersistentObject(GameObject obj)
    {
        if (!persistentObjects.Contains(obj))
        {
            DontDestroyOnLoad(obj);
            persistentObjects.Add(obj);
        }
    }

    public void RemovePersistentObject(GameObject obj)
    {
        if (persistentObjects.Contains(obj))
        {
            persistentObjects.Remove(obj);
        }
    }
}
