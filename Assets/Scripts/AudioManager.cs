using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public static AudioManager instance;

    private void Start()
    {
        instance = this;
        GetComponent<AudioSource>().clip = audioClips[0];
        GetComponent<AudioSource>().Play();
    }
}
