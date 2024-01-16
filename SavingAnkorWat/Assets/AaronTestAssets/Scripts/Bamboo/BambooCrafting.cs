using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable), typeof(Bamboo))]
public class BambooCrafting : MonoBehaviour
{
    [SerializeField] private int maxPieces = 6;
    [SerializeField] private float craftCheckPadding = 0.1f;
    [SerializeField] private InteractionLayerMask layerMaskAfterCompletion;
    [SerializeField] private float maxCraftLength = 0.5f;
    [SerializeField] private float textHeight = 1f;

    private XRGrabInteractable interactable;

    private bool isParentBamboo = false;
    public Bamboo Bamboo { get; private set; }
    private BambooCrafting parentBamboo;
    private Rigidbody rb;
    private TextMeshProUGUI textMeshProUGUI;

    private List<BambooCrafting> childBamboos = new List<BambooCrafting>();

    public bool isCrafted = false;

    private void Awake()
    {
        Bamboo = GetComponent<Bamboo>();
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<XRGrabInteractable>();
        interactable.selectExited.AddListener(OnRelease);
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

        textMeshProUGUI.enabled = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (isParentBamboo) return;
        Debug.Log("args.isCanceled: " + args.isCanceled);

        Vector3 center = transform.position + transform.up * Bamboo.BambooHeight * 0.5f;
        Vector3 halfExtents = Bamboo.BambooDimensions * 0.5f + Vector3.one * craftCheckPadding;
        Collider[] colliders = Physics.OverlapBox(center, halfExtents, transform.rotation);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out BambooCrafting other))
            {
                if (other == this) continue;

                bool craftingSuccessful = other.TryCrafting(this);

                if (craftingSuccessful) return;
            }
        }
    }

    public bool TryCrafting(BambooCrafting other)
    {
        if (!BambooCraftManager.Instance.AllowedToCraft) return false;

        if (!Bamboo.WasAlreadyChopped || !other.Bamboo.WasAlreadyChopped) return false;

        if (Bamboo.BambooHeight > maxCraftLength || other.Bamboo.BambooHeight > maxCraftLength) return false;

        if (isParentBamboo && childBamboos.Count >= maxPieces - 1) return false;

        if (parentBamboo != null) return parentBamboo.TryCrafting(other);


        Craft(other);

        return true;
    }

    private void Craft(BambooCrafting other)
    {
        isParentBamboo = true;
        isCrafted = true;
        other.isCrafted = true;
        childBamboos.Add(other);
        other.transform.SetParent(transform);

        float xPos;

        if (childBamboos.Count <= 1)
        {
            xPos = Bamboo.BambooRadius + other.Bamboo.BambooRadius;
        }
        else
        {
            BambooCrafting previousBamboo = childBamboos[childBamboos.Count - 2];
            xPos = previousBamboo.transform.localPosition.x + previousBamboo.Bamboo.BambooRadius + other.Bamboo.BambooRadius;
        }

        other.transform.localPosition = new Vector3(xPos, 0f, 0f);
        other.transform.localRotation = Quaternion.identity;

        other.SetParentBamboo(this);

        // activate TextMesh
        textMeshProUGUI.enabled = true;


        float totalWidth = Bamboo.BambooRadius * 2f;
        float totalHeight = Bamboo.BambooHeight;

        for (int i = 0; i < childBamboos.Count; i++)
        {
            totalWidth += childBamboos[i].Bamboo.BambooRadius * 2f;
            totalHeight += childBamboos[i].Bamboo.BambooHeight;
        }

        float colliderHeight = totalHeight / (childBamboos.Count + 1);
        float colliderThickness = totalWidth / (childBamboos.Count + 1);

        Bamboo.BoxCollider.size = new Vector3(totalWidth, colliderHeight, colliderThickness);
        Bamboo.BoxCollider.center = new Vector3((totalWidth * 0.5f) - Bamboo.BambooRadius, colliderHeight * 0.5f, 0f);

        // Handle TextMesh position
        textMeshProUGUI.transform.localPosition = new Vector3(totalWidth * 0.5f, colliderHeight + textHeight, - colliderThickness * 0.6f);

        // Set TextMesh text
        textMeshProUGUI.text = childBamboos.Count + 1 + " / " + maxPieces;

        if (childBamboos.Count == maxPieces - 1) HandleCraftingComplete();
    }

    public void SetParentBamboo(BambooCrafting parentBamboo)
    {
        this.parentBamboo = parentBamboo;

        rb.isKinematic = true;
        rb.detectCollisions = false;
        interactable.enabled = false;
    }

    private void HandleCraftingComplete()
    {
        interactable.interactionLayers = layerMaskAfterCompletion;
        BambooCraftManager.Instance.HandleCraftFinished(this);
    }
}
