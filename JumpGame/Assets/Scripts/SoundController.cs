using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : SingletonBehavior<SoundController> {

    public AudioClip[] sounds;
    AudioSource audioSource;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update ()
    {
		
	}

    public void PlaySound(int soundIndex)
    {
        if (soundIndex == sounds.Length)
        {
            audioSource.Play();
        }
        else
            audioSource.PlayOneShot(sounds[soundIndex]);
    }

    public void StopPlay()
    {
        audioSource.Stop();
    }
}
