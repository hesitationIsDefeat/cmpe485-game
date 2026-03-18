using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    private AudioSource bgmSource;

    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Pause(); 
            }
            else
            {
                bgmSource.Play(); 
            }
        }
    }
}
