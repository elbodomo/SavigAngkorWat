using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public List<AudioPart> audioParts = new List<AudioPart>();
    [SerializeField] AudioClip brtclip;
    private int curAudioIndex=0;
    AudioSource audioSource;
    private bool coroutineActive = false;
    private bool isHintReady = false;
    public bool isPressed = false;
    [SerializeField] private UnityEvent HintInfo;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(audioParts[curAudioIndex].isDone.ToString() + audioParts[curAudioIndex].isActive.ToString() + curAudioIndex);
        //if player solved puzzle 
        if (audioParts[curAudioIndex].isDone && audioParts[curAudioIndex].isActive)
        {
            StopCoroutine("TimeToHint");
            isHintReady = false;
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
                StartCoroutine("TimeToHint");
                coroutineActive = true;
            }
        }

        if (isPressed && isHintReady)
        {
            Debug.Log("play");
            audioSource.Play();
            isHintReady = false;
            isPressed = false;
            StartCoroutine("TimeToHint");
            coroutineActive = true;
        }
        else { isPressed = false; }

    }
    IEnumerator TimeToHint()
    {
        
       yield return new WaitForSeconds(audioParts[curAudioIndex].timeUntilHint);
        audioSource.clip = brtclip;
        audioSource.Play();
        yield return new WaitForSeconds(2);
        HintReady();
        
    }
    private void HintReady()
    {
        HintInfo.Invoke();
        audioSource.clip = audioParts[curAudioIndex].HintAudio;
        isHintReady = true;
    }
    
    public void Pressed()
    {
        Debug.Log("pressed");
        isPressed = true;
    }
}
