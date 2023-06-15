using UnityEngine;

public class MusicManager : MonoBehaviour {
    public string scene;
    private AudioSource audioSource;

    private void Start() 
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) 
        {
            Debug.Log("No AudioSource component found in this GameObject. Adding one.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        switch (scene)
        {
            case "MainArea":
                // Play the sound
                audioSource.Play();
                break;
        }
    }
}