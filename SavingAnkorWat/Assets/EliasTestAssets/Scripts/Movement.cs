using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] CharacterController characterCon;
    
    void Start()
    {
           characterCon = GetComponent<CharacterController>();  
    }

    // Update is called once per frame
    void Update()
    {
        characterCon.height = Camera.transform.position.y - transform.position.y;
        characterCon.center = transform.InverseTransformPoint(Camera.transform.position 
            - new Vector3(0, (Camera.transform.position.y - transform.position.y)/2, 0));
    }
}
