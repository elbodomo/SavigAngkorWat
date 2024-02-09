using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiHolder : MonoBehaviour
{
    public GameObject Binocular;
    public bool doRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resetPos() {
        Binocular.transform.position = this.gameObject.transform.position;
        if (doRotation) { 
        Binocular.transform.rotation = this.gameObject.transform.rotation;
        }
    }
}
