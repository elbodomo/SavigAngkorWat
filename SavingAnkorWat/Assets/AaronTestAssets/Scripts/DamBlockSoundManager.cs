using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class DamBlockSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip damPlacingSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource> ();
    }

    public void PlayAudioSource()
    {
        audioSource.PlayOneShot(damPlacingSound);
    }
}
