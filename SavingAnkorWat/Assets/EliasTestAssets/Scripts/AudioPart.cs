using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPart : MonoBehaviour
{
    [SerializeField] private string Name = string.Empty;

    public bool isActive = false;
    public bool isDone = false;
    public int timeUntilHint = 30;
    public AudioClip HintAudio;
    public AudioClip FeedbackAudio;

    // Update is called once per frame
    public void Done()
    {
        isDone = true;
    }
}
