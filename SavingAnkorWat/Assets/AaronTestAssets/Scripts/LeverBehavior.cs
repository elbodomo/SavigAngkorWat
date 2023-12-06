using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBehavior : MonoBehaviour
{
    [SerializeField] private UnityEvent LeverWasUsed;

    [SerializeField] private Transform stoneTransform;
    [SerializeField] private Transform stoneOrientationTransform;

    private Vector3 stoneOrientationStartPos;
    private Vector3 stonePositionStart;
    private float leverHeightValue;

    private void Awake()
    {
        stoneOrientationStartPos = stoneOrientationTransform.position;
        stonePositionStart = stoneTransform.position;
    }

    private void Update()
    {
        if (!stoneTransform.gameObject.activeSelf) return;
        leverHeightValue = stoneOrientationTransform.position.y - stoneOrientationStartPos.y;
        stoneTransform.position = new Vector3(stoneTransform.position.x, stonePositionStart.y + leverHeightValue, stoneTransform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out LeverTrigger leverTrigger))
        {
            LeverWasUsed.Invoke();
        }
    }
}
