using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BambooCraftManager : MonoBehaviour
{
    [SerializeField] private int maxCraftCount = 3;
    [SerializeField] private UnityEvent onMaxDamsCrafted;

    public UnityEvent onBambooChoppedSmallEnough;


    public static BambooCraftManager Instance;

    public bool AllowedToCraft => craftCounter < maxCraftCount;

    private int craftCounter = 0;
    private bool craftCountReached;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (craftCountReached == true) return;
          if (craftCounter >= maxCraftCount)
        {
            onMaxDamsCrafted?.Invoke();
            craftCountReached = true;
        }
    }

    public void HandleCraftFinished(BambooCrafting bambooCrafting)
    {
        if (!AllowedToCraft)
        {
            Debug.LogWarning("You are not allowed to craft! something went wrong!");
            return;
        }

        craftCounter++;
    }

    public void CraftDebug()
    {
        Debug.Log("all dams crafted event");
    }

    public void ChopedSmallEnoughDebug()
    {
        Debug.Log("Choped small enough event");
    }
}
