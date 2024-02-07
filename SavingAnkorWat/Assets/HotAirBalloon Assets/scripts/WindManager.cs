using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    [SerializeField] private float minVolume = 0f;
    [SerializeField, Range(0, 1)] private float maxVolume = 1f;

    private BalloonMovementController balloonMovementController;
    private AudioSource audioSource;
    private float soundVolume;


    private void Awake()
    {
        balloonMovementController = FindFirstObjectByType<BalloonMovementController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        soundVolume = RemapFloat(balloonMovementController.BalloonPosY, balloonMovementController.GroundHeight, balloonMovementController.MaxPositionY, minVolume, maxVolume);
        audioSource.volume = soundVolume;
        //Debug.Log("volume = " + audioSource.volume);
    }

    private float RemapFloat(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float t = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, t);
    }
}
