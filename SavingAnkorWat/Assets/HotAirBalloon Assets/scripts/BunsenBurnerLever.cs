using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BunsenBurnerLever : MonoBehaviour
{
    /*
    private float LeverRot;

    public GameObject BunsenBurner;
    public GameObject destination;

    public Material darkgrey;
    public Material red;
    public Material redgray;

    public bool reachGoal = false;

    private float verticalSpeed = 0.1f;


    // Start is called before the first frame update 
    void Start()
    {
        BunsenBurner.GetComponent<Renderer>().material = darkgrey;
    }

    // Update is called once per frame 
    void Update()
    {
        if (!reachGoal)
        {
            LeverRot = transform.localRotation.z; //angle rotation from the lever
            Debug.Log(LeverRot);


            if ((LeverRot <= 0.3f) && (destination.transform.position.y > -60.0f)) //lifting the lever up
            {
                BunsenBurner.GetComponent<Renderer>().material = red;
                destination.transform.Translate(Vector3.down * verticalSpeed);

                //Debug.Log("material is now red");
            }
            else if ((LeverRot >= 0.8f) && (destination.transform.position.y < -20.0f)) //lifting the lever down
            {
                BunsenBurner.GetComponent<Renderer>().material = redgray;
                destination.transform.Translate(Vector3.up * verticalSpeed);
                //Debug.Log("material is now redgray");
            }
            else //lever in original position kinda
            {
                BunsenBurner.GetComponent<Renderer>().material = darkgrey;
                destination.transform.Translate(new Vector3(0, 0, 0) * verticalSpeed);
                //Debug.Log("material is now darkgray");
            }
        }
        

    }
    */

    private float LeverRot;

    public GameObject BunsenBurner;
    public GameObject destination;

    public Material darkgrey;
    public Material red;
    public Material redgray;

    public bool reachGoal = false;

    [SerializeField] private GameObject hotAirBallon;

    private float verticalSpeed = 0.03f;


    // Start is called before the first frame update 
    void Start()
    {
        BunsenBurner.GetComponent<Renderer>().material = darkgrey;
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        if (!reachGoal)
        {
            LeverRot = transform.localRotation.z; //angle rotation from the lever
            Debug.Log(LeverRot);


            if ((LeverRot <= 0.3f) && (destination.transform.position.y > -60.0f)) //lifting the lever up
            {
                BunsenBurner.GetComponent<Renderer>().material = red;
                //destination.transform.Translate(Vector3.down * verticalSpeed);
                hotAirBallon.transform.Translate(Vector3.down * verticalSpeed);

                //Debug.Log("material is now red");
            }
            else if ((LeverRot >= 0.8f) && (destination.transform.position.y < -20.0f)) //lifting the lever down
            {
                BunsenBurner.GetComponent<Renderer>().material = redgray;
                //destination.transform.Translate(Vector3.up * verticalSpeed);
                hotAirBallon.transform.Translate(Vector3.up * verticalSpeed);
                //Debug.Log("material is now redgray");
            }
            else //lever in original position kinda
            {
                BunsenBurner.GetComponent<Renderer>().material = darkgrey;
                //destination.transform.Translate(new Vector3(0, 0, 0) * verticalSpeed);
                hotAirBallon.transform.Translate(Vector3.up * verticalSpeed);

                //Debug.Log("material is now darkgray");
            }
        }
    }
}
