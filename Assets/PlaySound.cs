using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip soundFX;

    AudioSource audioSource;

    void Awake(){
    audioSource = GetComponent<AudioSource>();
    audioSource.clip = soundFX;
    }

    public void SoundEvent(){
    audioSource.Play();
    }
}
