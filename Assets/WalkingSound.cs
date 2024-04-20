using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WalkingSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    AudioSource audioSource;

    void Awake (){
    audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        int randomInt = Random.Range(1, audioClips.Length);
        audioSource.clip = audioClips[randomInt];
        audioSource.Play();
    }

    void Update(){
        if (Input.GetKey(InputMappings.Run)){
            PlaySound();        
        }
    }
}
