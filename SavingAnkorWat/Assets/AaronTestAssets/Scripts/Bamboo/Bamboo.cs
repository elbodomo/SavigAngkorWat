using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable), typeof(BoxCollider))]
[RequireComponent (typeof(AudioSource))]
public class Bamboo : MonoBehaviour, IChoppable
{
    [SerializeField] private float minHeight = 2f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float minRadius = 0.04f;
    [SerializeField] private float maxRadius = 0.06f;

    [SerializeField] private float minChopVelocity;
    [SerializeField] private float minChopLength = 0.2f;

    [SerializeField] private Transform visuals;

    [SerializeField] private AudioClip[] chopAudioFiles;

    private AudioSource audioSource;
    private Rigidbody rb;
    private XRGrabInteractable xrGrabInteractable;
    private bool disableResizing = false;

    // branches
    [SerializeField] private BambooBranch[] branchPrefabs;
    [SerializeField] private float minBranchScale = 0.8f;
    [SerializeField] private float maxBranchScale = 1.2f;
    [SerializeField, Range(-90f, 90f)] private float minBranchRotationZ = -30f;
    [SerializeField, Range(-90f, 90f)] private float maxBranchRotationZ = 30f;
    [SerializeField] private float branchBotOffset = 0.2f;
    [SerializeField] private float branchTopOffset = 0.05f;
    [SerializeField] private int minBranchCount;
    [SerializeField] private int maxBranchCount;

    private List<BambooBranch> branchInstances = new List <BambooBranch>();



    public float MinimumChopVelocity => 6f;
    public float BambooRadius { get; private set; }
    public float BambooHeight { get; private set; }
    public Vector3 BambooDimensions => new Vector3(BambooRadius, BambooHeight, BambooRadius);
    public BoxCollider BoxCollider { get; private set; }
    public bool WasAlreadyChopped { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        if(WasAlreadyChopped) return;
        InstatiateBranches();
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

        // play chopSound
        PlayBambooBreakSound(audioSource, chopAudioFiles);


        // let Branches fall
        for (int i = branchInstances.Count - 1; i >= 0; i--)
        {
            if (branchInstances[i].transform.localPosition.y >= chopLength)
            {
                branchInstances[i].Release();
                branchInstances.RemoveAt(i);
            }
            else
            {
                branchInstances[i].transform.SetParent(null, true);
            }
        }

        GameObject newBambooGO = Instantiate(gameObject, spawnPosition, transform.rotation);

        for (int i = branchInstances.Count - 1; i >= 0; i--)
        {
            branchInstances[i].transform.SetParent(transform, true);
        }


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

    private void InstatiateBranches()
    {
        int branchCount = Random.Range(minBranchCount, maxBranchCount);
        for (int i = 0; i < branchCount; i++)
        {
            int randomPrefabIndex = Random.Range(0, branchPrefabs.Length);
            float branchHeight = Random.Range(branchBotOffset, BambooHeight - branchTopOffset);
            float branchRotationZ = Random.Range(minBranchRotationZ, maxBranchRotationZ);

            BambooBranch branch = Instantiate(branchPrefabs[randomPrefabIndex], transform);

            Vector3 lookDirection = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward;
            Vector3 localPos = lookDirection * BambooRadius;
            localPos.y = branchHeight;
            branch.transform.localPosition = localPos;

            branch.transform.localRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            branch.transform.Rotate(transform.right, branchRotationZ);

            float randomBranchScale = Random.Range(minBranchScale, maxBranchScale);
            branch.transform.localScale = Vector3.one * randomBranchScale;

            branchInstances.Add(branch);
        }
    }

    private void PlayBambooBreakSound(AudioSource audioSource, AudioClip[] audioClipArray)
    {
        int randomClipArray = Random.Range(0, audioClipArray.Length);
        audioSource.PlayOneShot(audioClipArray[randomClipArray]);
    }
}