using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonMovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private HingeJoint leverJoint;
    [SerializeField] private ParticleSystem[] fireParticle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private float leverDeadzoneAngle = 15f;
    [SerializeField] private float balloonVerticalSpeed = 30f;
    [SerializeField] private float balloonHorizontalSpeed = 20f;
    [SerializeField] private float maxPositionY = 30f;
    [SerializeField] private float minPositionY = 20f;
    [SerializeField] private float treshold = 0.1f;

    private bool isEmitting;

    
    private void FixedUpdate()
    {
        Vector3 targetXZ = target.position;
        targetXZ.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetXZ, balloonHorizontalSpeed * Time.fixedDeltaTime);

        float verticalMovement = 0f;

        if(Mathf.Abs(leverJoint.angle) > leverDeadzoneAngle)
        {
            bool isRising = leverJoint.angle < 0f;

            float value = leverJoint.angle;
            float fromMin = isRising ? -leverDeadzoneAngle : leverDeadzoneAngle;
            float fromMax = isRising ? leverJoint.limits.min : leverJoint.limits.max;
            float toMin = 0f;
            float toMax = isRising ? balloonVerticalSpeed : -balloonVerticalSpeed;

            verticalMovement = RemapFloat(value, fromMin, fromMax, toMin, toMax);

        }

        Vector3 finalPosition = transform.position + Vector3.up * verticalMovement * Time.fixedDeltaTime;

        if (IsDestinationReached(transform.position, targetXZ))
        {
            minPositionY = target.position.y;

            if(IsDestinationReached(transform.position, target.position)) 
            {
                ChangeScene();
            }
        }

        finalPosition.y = Mathf.Clamp(finalPosition.y, minPositionY, maxPositionY);

        transform.position = finalPosition;

        HandleFire();

        //Physics.SyncTransforms();
        }
    
    /*
    private void Update()
    {
        Vector3 targetXZ = target.position;
        targetXZ.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetXZ, balloonHorizontalSpeed * Time.deltaTime);

        float verticalMovement = 0f;

        if (Mathf.Abs(leverJoint.angle) > leverDeadzoneAngle)
        {
            bool isRising = leverJoint.angle < 0f;

            float value = leverJoint.angle;
            float fromMin = isRising ? -leverDeadzoneAngle : leverDeadzoneAngle;
            float fromMax = isRising ? leverJoint.limits.min : leverJoint.limits.max;
            float toMin = 0f;
            float toMax = isRising ? balloonVerticalSpeed : -balloonVerticalSpeed;

            verticalMovement = RemapFloat(value, fromMin, fromMax, toMin, toMax);

        }

        Vector3 finalPosition = transform.position + Vector3.up * verticalMovement * Time.deltaTime;

        if (IsDestinationReached(transform.position, targetXZ))
        {
            minPositionY = target.position.y;

            if (IsDestinationReached(transform.position, target.position))
            {
                ChangeScene();
            }
        }

        finalPosition.y = Mathf.Clamp(finalPosition.y, minPositionY, maxPositionY);

        transform.position = finalPosition;

        HandleFire();

        Physics.SyncTransforms();
    } */

    private float RemapFloat(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float t = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, t);
    }

    private void HandleFire()
    {
        if (leverJoint.angle <= -leverDeadzoneAngle)
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
        SceneManager.LoadScene("TestLevel1", LoadSceneMode.Single);
    }

    private void PlayFireAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(audioClip);
    }
}