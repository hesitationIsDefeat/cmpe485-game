using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] punchSwings; 
    [SerializeField] private AudioClip[] punchHits;    
    [SerializeField] private AudioClip[] blockImpacts; 

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 2f;
        audioSource.maxDistance = 15f;
    }

    public void PlaySwingSound()
    {
        if (punchSwings.Length > 0)
        {
            AudioClip clip = punchSwings[Random.Range(0, punchSwings.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayHitSound()
    {
        if (punchHits.Length > 0)
        {
            AudioClip clip = punchHits[Random.Range(0, punchHits.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayBlockSound()
    {
        if (blockImpacts.Length > 0)
        {
            AudioClip clip = blockImpacts[Random.Range(0, blockImpacts.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
