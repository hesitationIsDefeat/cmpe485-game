using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager INSTANCE;

    [Header("Sound Options")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    void Awake() {
        if (INSTANCE = null) {
            INSTANCE = this;
        } else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            ToggleAudio();
        }
    }

    void ToggleAudio() {
        if (audioSource.isPlaying) {
            audioSource.Stop();
        } else {
            if (audioSource.clip == null) {
                audioSource.clip = audioClip;
            }
            audioSource.Play();
        }
    }
}
