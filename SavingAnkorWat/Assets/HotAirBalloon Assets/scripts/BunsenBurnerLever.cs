using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunsenBurnerLever : MonoBehaviour
{
    private float LeverRot;

    public GameObject BunsenBurner;

    public Material darkgrey;
    public Material red;
    public Material redgray;
    



    // Start is called before the first frame update 
    void Start()
    {
        BunsenBurner.GetComponent<Renderer>().material = darkgrey;
    }

    // Update is called once per frame 
    void Update()
    {
        LeverRot = transform.localRotation.z; //angle rotation from the lever
        Debug.Log(LeverRot);

        
        if (LeverRot <= 0.3f) //lifting the lever up
        {
            BunsenBurner.GetComponent<Renderer>().material = red;
            Debug.Log("material is now red");
        }
        else if (LeverRot >= 0.8f ) //lifting the lever down
        {
            BunsenBurner.GetComponent<Renderer>().material = redgray;
            Debug.Log("material is now redgray");
        }
        else //lever in original position kinda
        {
            BunsenBurner.GetComponent<Renderer>().material = darkgrey;
            Debug.Log("material is now darkgray");
        }

    }
}
