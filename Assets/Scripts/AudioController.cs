using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Controls audio and stuff
*/


public class AudioController : MonoBehaviour
{
    public AudioClip startBGM;
    public AudioClip normalBGM;
    public AudioClip scaredBGM;
    public AudioClip oneEatenBGM;

    public AudioSource audioSource;

    void Start()
    {
        audioSource.PlayOneShot(startBGM);
        audioSource.clip = normalBGM;
        audioSource.loop = true;
        audioSource.PlayDelayed(startBGM.length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
