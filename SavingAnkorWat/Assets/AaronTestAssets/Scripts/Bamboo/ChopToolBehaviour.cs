using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ChopToolBehaviour : MonoBehaviour
{
    [SerializeField] private Transform bladeTransform;
    [SerializeField] private AudioClip[] chopSoundClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoints);

        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (contactPoints[i].thisCollider.transform != bladeTransform) continue;
            {
                if (collision.gameObject.TryGetComponent(out IChoppable choppable))
                {
                    PlayChopSound(audioSource, chopSoundClips);

                    if (collision.relativeVelocity.magnitude < choppable.MinimumChopVelocity) return;

                    choppable.Chop(collision);
                }
            }
        }
    }
    private void PlayChopSound(AudioSource audioSource, AudioClip[] audioClipArray)
    {
        if (audioSource.isPlaying) return;
        int randomClipArray = Random.Range(0, audioClipArray.Length);

        audioSource.PlayOneShot(audioClipArray[randomClipArray]);
    }
}
