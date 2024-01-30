using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(AudioSource))]
public class BambooBranch : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 5f;
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource audioSource;
    private Rigidbody rb;
    private Collider branchCollider;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        branchCollider = GetComponent<Collider>();
        branchCollider.enabled = false;
    }

    public void Release()
    {
        transform.parent = null;
        rb.isKinematic = false;
        branchCollider.enabled = true;

        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Terrain terrain))
        {
            PlayAudioClip(audioSource, audioClips);
        }
    }

    private void PlayAudioClip(AudioSource audioSource, AudioClip[] audioClips)
    {
        if (audioClips == null) return;
        if (audioSource.isPlaying) return;
        int randomAudioClip = Random.Range(0, audioClips.Length);

        audioSource.PlayOneShot(audioClips[randomAudioClip]);
    }
}
