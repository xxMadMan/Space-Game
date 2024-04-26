using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WalkingSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    AudioSource audioSource;

    [HideInInspector]
    public float waitTime;

    public float walkingInterval = 0.6f;
    public float runningInterval = 0.2f;

    public bool isMoving = false;

    [HideInInspector]
    public float volume = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start(){
        StartCoroutine(StartSound());
    }

    private IEnumerator StartSound(){
        yield return new WaitUntil(() => isMoving);
        yield return new WaitForSeconds(waitTime);
        int randomInt = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomInt];
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(StartSound());
    }
}
