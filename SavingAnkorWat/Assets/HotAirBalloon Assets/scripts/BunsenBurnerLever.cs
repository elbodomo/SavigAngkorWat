using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunsenBurnerLever : MonoBehaviour
{
    private float LeverRot;
    public float LeverRange = 30.0f;

    public GameObject BunsenBurner;
    private Material BunsenBurnerMat;

    public Material darkgrey;
    public Material redgray;
    public Material red;

    // Start is called before the first frame update
    void Start()
    {
        BunsenBurnerMat = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        LeverRot = transform.localRotation.z;

        if (LeverRot <= LeverRange)
        {
            BunsenBurnerMat = redgray;
        }
        else if (LeverRot >= LeverRange)
        {
            BunsenBurnerMat = red;
        }
        else
        {
            BunsenBurnerMat = darkgrey;
        }

    }
}
