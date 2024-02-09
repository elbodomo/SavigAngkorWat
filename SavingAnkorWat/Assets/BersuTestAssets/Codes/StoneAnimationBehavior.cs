using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneAnimationBehavior : MonoBehaviour
{

    
    [SerializeField]
    private AudioSource audioSource1;
    [SerializeField]
    private AudioSource audioSource2;
    [SerializeField]
    private AudioSource audioSource3;



    //[SerializeField]
    //private GameObject leak;
    //[SerializeField]
    //private GameObject outflow;
    [SerializeField]
    private UnityEvent StoneCrashed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void playSoundFile1st()
    {
        audioSource1.Play();
    }
    void playSoundFile2st()
    {
        audioSource2.Play();
    }
    void playSoundSile3rd()
    {
        audioSource3.Play();
    }
    void stoneCrashesTheWateSystem()
    {
        //        particles.SetActive(true);
        //       outflow.SetActive(false); 
        //       leak.SetActive(true);
        StoneCrashed.Invoke();
    }


}
