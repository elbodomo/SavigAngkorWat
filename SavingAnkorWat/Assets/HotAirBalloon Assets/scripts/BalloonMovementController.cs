using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class BalloonMovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private HingeJoint leverJoint;
    [SerializeField] private ParticleSystem[] fireParticle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private float leverDeadzoneAngle = 15f;
    [SerializeField] private float balloonVerticalSpeed = 30f;
    [SerializeField] private float maxBalloonHorizontalSpeed = 20f;
    [SerializeField] private float maxPositionY = 30f;
    [SerializeField] private float minPositionY = 20f;
    [SerializeField] private float treshold = 0.1f;
    [SerializeField] private float speedIncreaseValue = 0.2f;
    [SerializeField] private float groundHeight = 58f;

    private bool isEmitting;
    private float minFlyHeight;
    private float currentHorizontalSpeed;

    public float BalloonPosY => transform.position.y;
    public float MaxPositionY => maxPositionY;
    public float MinPositionY => minPositionY;
    public float GroundHeight => groundHeight;



    private void Start()
    {
        minFlyHeight = groundHeight;
    }

    private void FixedUpdate()
    {
        Vector3 targetXZ = target.position;
        targetXZ.y = transform.position.y;

        if (transform.position.y > minPositionY - treshold)
        {
            currentHorizontalSpeed = currentHorizontalSpeed + speedIncreaseValue * Time.fixedDeltaTime;
            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, 0, maxBalloonHorizontalSpeed);
            transform.position = Vector3.MoveTowards(transform.position, targetXZ, currentHorizontalSpeed  * Time.fixedDeltaTime);
            //Debug.Log(currentHorizontalSpeed);
        }

        float verticalMovement = 0f;

        if(Mathf.Abs(leverJoint.angle) > leverDeadzoneAngle)
        {
            bool isRising = leverJoint.angle < 0f;

            float value = leverJoint.angle;
            float fromMin = isRising ? -leverDeadzoneAngle : leverDeadzoneAngle;
            float fromMax = isRising ? leverJoint.limits.min : leverJoint.limits.max;
            float toMin = 0f;
            float toMax = isRising ? -balloonVerticalSpeed : balloonVerticalSpeed;

            verticalMovement = RemapFloat(value, fromMin, fromMax, toMin, toMax);

        }

        // Reset minPosY
        if (IsDestinationReached(transform.position, targetXZ))
        {
            minFlyHeight = target.position.y;

        }
        else if (transform.position.y > minPositionY)
        {
            minFlyHeight = minPositionY;
        }

        if (IsDestinationReached(transform.position, target.position))
        {
            ChangeScene();
        }

        Vector3 finalPosition = transform.position + Vector3.up * verticalMovement * Time.fixedDeltaTime;

        finalPosition.y = Mathf.Clamp(finalPosition.y, minFlyHeight, maxPositionY);

        transform.position = finalPosition;

        HandleFire();

        //Physics.SyncTransforms();
        }
    

    private float RemapFloat(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float t = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, t);
    }

    private void HandleFire()
    {
        if (BalloonIsRising())
        {
            for (int i = 0; i < fireParticle.Length; i++)
            {
                fireParticle[i].Play();
            }
            isEmitting = true;
            PlayFireAudio(audioSource, fireAudio);
        }
        else if (isEmitting)
        {
            for (int i = 0; i < fireParticle.Length; i++)
            {
                fireParticle[i].Stop();
                audioSource.Stop();
            }
        }
    }

    private bool IsDestinationReached(Vector3 transform, Vector3 target)
    {
        if(Vector3.Distance(transform, target) < treshold)
        {
            return true;
        }
        else return false;
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("NewLevelWaterSystem", LoadSceneMode.Single);
    }

    private void PlayFireAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(audioClip);
    }

    private bool BalloonIsRising()
    {
        if (leverJoint.angle >= leverDeadzoneAngle)
        {
            return true;
        }
        else return false;
    }
}