using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneAnimationBehavior : MonoBehaviour
{

    //sound1
    //sound2
    //sound3
    //[SerializeField]
    //private GameObject particles;
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
        //playSoundFile1st
    }
    void playSoundFile2st()
    {
        //playSoundFile2nd        
    }
    void playSoundSile3rd()
    {
        //playSoundFile3rd
    }
    void stoneCrashesTheWateSystem()
    {
        //        particles.SetActive(true);
        //       outflow.SetActive(false); 
        //       leak.SetActive(true);
        StoneCrashed.Invoke();
    }


}
