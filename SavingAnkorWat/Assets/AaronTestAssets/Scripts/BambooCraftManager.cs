using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooCraftManager : MonoBehaviour
{
    [SerializeField] private int maxCraftCount = 3;

    public static BambooCraftManager Instance;

    public bool AllowedToCraft => craftCounter < maxCraftCount;

    private int craftCounter = 0;

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

    public void HandleCraftFinished(BambooCrafting bambooCrafting)
    {
        if (!AllowedToCraft)
        {
            Debug.LogWarning("You are not allowed to craft! something went wrong!");
            return;
        }

        craftCounter++;
    }
}
