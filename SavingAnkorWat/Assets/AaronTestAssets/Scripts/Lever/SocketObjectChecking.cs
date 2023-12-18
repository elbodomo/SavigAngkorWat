using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketObjectChecking : MonoBehaviour
{
    XRSocketInteractor socket;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    public void DeleteSocketObject()
    {
        IXRSelectInteractable socketInteractionObj = socket.GetOldestInteractableSelected();
        GameObject interactionObj = socketInteractionObj.transform.gameObject;
        interactionObj.SetActive(false);
    }
}
