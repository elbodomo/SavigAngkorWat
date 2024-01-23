using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioPart> audioParts = new List<AudioPart>();
    private int curAudioIndex=0;
    AudioSource audioSource;
    private bool coroutineActive = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(audioParts[curAudioIndex].isDone.ToString() + audioParts[curAudioIndex].isActive.ToString() + curAudioIndex);
        //if player solved puzzle 
        if (audioParts[curAudioIndex].isDone && audioParts[curAudioIndex].isActive)
        {
            StopCoroutine("PlayHint");
            coroutineActive = false;
            audioParts[curAudioIndex].isActive = false;
            audioSource.clip = audioParts[curAudioIndex].FeedbackAudio;
            audioSource.Play();
        }
        else if (audioParts[curAudioIndex].isDone == false && audioParts[curAudioIndex].isActive == false)
        {
            audioParts[curAudioIndex].isActive = true;
        }// skip hint
        else if (audioParts[curAudioIndex].isDone == true && audioParts[curAudioIndex].isActive == false)
        {
            curAudioIndex++;
        }
        //play Hint
        else if (audioParts[curAudioIndex].isDone == false && audioParts[curAudioIndex].isActive == true)
        {
            if (coroutineActive == false)
            {
                StartCoroutine("PlayHint");
                coroutineActive = true;
            }
        }

    }
    IEnumerator PlayHint()
    {
        while(true)
        {
            yield return new WaitForSeconds(audioParts[curAudioIndex].timeUntilHint);
            audioSource.clip = audioParts[curAudioIndex].HintAudio;
            audioSource.Play();
        }
    }
}
