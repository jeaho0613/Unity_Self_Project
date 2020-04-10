using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public AudioClip[] fxClips;
    public AudioClip[] bgmClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClips[0];
        audioSource.Play();
    }

}
