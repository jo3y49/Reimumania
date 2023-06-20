using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private float storedTime = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PauseAudio()
    {
        storedTime = audioSource.time;
        audioSource.Pause();
    }

    public void UnpauseAudio()
    {
        audioSource.time = storedTime;
        audioSource.UnPause();
    }
}
