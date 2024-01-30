using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BambooBranch : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 5f;
    private Rigidbody rb;
    private Collider branchCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        branchCollider = GetComponent<Collider>();
        branchCollider.enabled = false;
    }

    public void Release()
    {
        transform.parent = null;
        rb.isKinematic = false;
        branchCollider.enabled = true;

        Destroy(gameObject, destroyDelay);
    }
}
