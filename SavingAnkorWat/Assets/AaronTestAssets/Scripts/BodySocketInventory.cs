using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BodySocket
{
    public GameObject gameObject;
    [Range(0f, 1f)] public float heightRatio;
}

public class BodySocketInventory : MonoBehaviour
{
    public CharacterController character;
    public GameObject head;
    public BodySocket[] bodySockets;
    

    private Vector3 currentHeadPos;
    private Quaternion currentHeadRot;
    private void Update()
    {
        currentHeadPos = head.transform.position;
        currentHeadRot = head.transform.rotation;
        foreach (var BodySocket in bodySockets)
        {
            UpdateBodySocketHeight(BodySocket);
        }
        UpdateSocketInventory();
    }

    private void UpdateBodySocketHeight(BodySocket bodySocket)
    {
        bodySocket.gameObject.transform.position = new Vector3(bodySocket.gameObject.transform.position.x,currentHeadPos.y - character.height + character.height * bodySocket.heightRatio, bodySocket.gameObject.transform.position.z);
    }

    private void UpdateSocketInventory()
    {
        transform.position = new Vector3(currentHeadPos.x, 0, currentHeadPos.z);
        transform.rotation = new Quaternion(transform.rotation.x, currentHeadRot.y, transform.rotation.z, currentHeadRot.w);
    }
}
