using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable), typeof(BoxCollider))]
public class Bamboo : MonoBehaviour, IChoppable
{
    [SerializeField] private float minHeight = 2f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float minRadius = 0.04f;
    [SerializeField] private float maxRadius = 0.06f;

    [SerializeField] private float minChopVelocity;
    [SerializeField] private float minChopLength = 0.2f;

    [SerializeField] private Transform visuals;

    private Rigidbody rb;
    private XRGrabInteractable xrGrabInteractable;
    private bool disableResizing = false;

    public float MinimumChopVelocity => 6f;
    public float BambooRadius { get; private set; }
    public float BambooHeight { get; private set; }
    public Vector3 BambooDimensions => new Vector3(BambooRadius, BambooHeight, BambooRadius);
    public BoxCollider BoxCollider { get; private set; }
    public bool WasAlreadyChopped { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        BoxCollider = GetComponent<BoxCollider>();
        rb.isKinematic = true;
        rb.useGravity = false;
        xrGrabInteractable.enabled = false;
        WasAlreadyChopped = false;
    }

    private void Start()
    {
        if (disableResizing) return;

        BambooRadius = Random.Range(minRadius, maxRadius);

        SetHeight(Random.Range(minHeight, maxHeight));
    }
    public void OnSpawnedAfterChop()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        disableResizing = true;
        xrGrabInteractable.enabled = true;
        WasAlreadyChopped = true;
    }

    public void Chop(Collision collision)
    {
        // Only Chop when not grabbed
        if (xrGrabInteractable.isSelected) return;

        // Only chop when not crafted
        if (collision.gameObject.TryGetComponent(out BambooCrafting bambooCrafting))
        {
            if (bambooCrafting.isCrafted) return;
        }

        // Set chop Point
        Vector3 chopPoint = collision.contacts[0].point;

        // Set chop length (world to local)
        float chopLength = transform.InverseTransformPoint(chopPoint).y;

        // return if bottom piece would be too short
        if (chopLength < minChopLength) return;

        Debug.Log($"total: {BambooHeight} / chop: {chopLength} / diff: {BambooHeight - chopLength}");

        // return if top piece would be too short
        if (BambooHeight - chopLength < minChopLength) return;

        // spawn new bamboo piece at chop point
        Vector3 spawnPosition = transform.position + transform.up * chopLength;

        GameObject newBambooGO = Instantiate(gameObject, spawnPosition, transform.rotation);

        if (newBambooGO.TryGetComponent(out Bamboo newBamboo))
        {
            newBamboo.SetBambooRadius(BambooRadius);
            newBamboo.SetHeight(BambooHeight - chopLength);
            newBamboo.OnSpawnedAfterChop();

            // Check for small peace to play event
            if (newBamboo.BambooHeight < newBamboo.GetComponent<BambooCrafting>().MaxCraftLenght)
            {
                BambooCraftManager.Instance.onBambooChoppedSmallEnough?.Invoke();
            }
        }

        // make own bamboo piece shorter
        SetHeight(chopLength);
    }

    public void SetHeight(float height)
    {
        BambooHeight = height;

        visuals.localScale = new Vector3(BambooRadius * 2f, height, BambooRadius * 2f);
        BoxCollider.center = new Vector3(0f, height * 0.5f, 0f);
        BoxCollider.size = new Vector3(BambooRadius * 2f, height, BambooRadius * 2f);
    }

    public void SetBambooRadius(float radius)
    {
        BambooRadius = radius;
    }
}