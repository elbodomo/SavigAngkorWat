using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    public InputActionProperty grip;
    public InputActionProperty index;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Grip",grip.action.ReadValue<float>());
        animator.SetFloat("Index", index.action.ReadValue<float>());
    }
}
