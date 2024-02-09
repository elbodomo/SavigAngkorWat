using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverFlowBLocked : MonoBehaviour
{
    private bool block1=false, block2=false;
    [SerializeField] private UnityEvent bothBlocked;
    private void checkStatus()
    {
        if (block1&&block2)
        {
            bothBlocked.Invoke();
        }

    }
    public void doBlock1() { block1 = true; checkStatus(); }
    public void doBlock2() { block2 = true; checkStatus(); }
    public void undoBlock1() { block1 = false; }
    public void undoBlock2() { block2 = false; }

}
