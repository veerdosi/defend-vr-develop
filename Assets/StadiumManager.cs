using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadiumManager : MonoBehaviour
{
    public AudioClip crowdSound; // The crowd sound to be played
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip and configure the AudioSource
        audioSource.clip = crowdSound;
        audioSource.loop = true; // Ensure the sound loops continuously
        audioSource.playOnAwake = true; // Optionally play the sound immediately on awake

        // Play the crowd sound
        audioSource.Play();
    }
}
