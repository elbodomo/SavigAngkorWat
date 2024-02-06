using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaterScript : MonoBehaviour
{
    
    public float WaterLevel = 0;
    public float WaterMax = 30;
    public float WaterPercentage = 1;
    public float minimumFlowPercentage = 1;
    [SerializeField] private float WaterOutMin = 10;
    [SerializeField] private float SourceFlow = 3f;
    [SerializeField] private bool isSource = false;
    [SerializeField] private float DrainFlow = 2f;
    [SerializeField] private bool isDrain = false;

    [SerializeField] private float OutflowMinPercentage = 0f;
    [SerializeField] private float LimitPercentage = 10f;
    [SerializeField] private float DrainLimitPercentage = 10f;
    [SerializeField] private float LeakLimitPercentage = 15f;
    [SerializeField] private float WaterVisualOffset = 0.16f;
    [SerializeField] private float WaterVisualSize = 0.2f;
    [SerializeField] private bool isLeak = false;
    public bool isBlocked = false;
    [SerializeField] private GameObject waterLevel;

    [SerializeField] private UnityEvent LeakCleared;
    public bool isFixed = false;
    public GameObject blockade;
    public float OutFlowRate = 10f;
    float timeSinceLastValue = 0f;
    int waterVisualsCounter = 0;
    float WaterLevelY;
    float lastWaterLevelY = 0;
    float currentWaterLevelY = 0;
    [SerializeField] List<GameObject> OverflowVisual = new List<GameObject>();
    [SerializeField] List<GameObject> InPuts = new List<GameObject>();
    [SerializeField] List<GameObject> OutPuts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        WaterLevelY = waterLevel.transform.position.y;
        foreach(GameObject outPut in OutPuts)
        {
            outPut.GetComponent<WaterScript>().InPuts.Add(this.gameObject);
        } 
    }
    int count = 0;
    int CountGoal=3;
    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (count >= CountGoal) { */
        WaterPercentage = WaterLevel / WaterMax * 100;
        /*if (isBlocked && blockade.activeSelf == false)
        {
            blockade.SetActive(true);
        }
        if (isBlocked == false && blockade.activeSelf )
        {
            blockade.SetActive(false);
        }*/

        if (isSource)
        {
            Source();
        }
        if (isDrain)
        {
            Drain();
        }
        if (isLeak)
        {
            Leak();
        }
        if (WaterLevel >= WaterMax)
        {
            WaterLevel = WaterMax;
        }
        if (InPuts != null && isBlocked==false && isLeak == false)
        {
            BackFlow();
        }
        if(WaterPercentage >= OutflowMinPercentage) { 
            OutPutFlow();
        }
            /*count = 0;
        }
        count++;*/
        UpdateWaterLevel();    
    }
    void UpdateWaterLevel()
    {
        if (isSource)
        {
            WaterLevelY += ((OutPuts[0].GetComponent<WaterScript>().WaterLevel / OutPuts[0].GetComponent<WaterScript>().WaterMax * WaterVisualSize) + transform.position.y + WaterVisualOffset);
        }
        else { 
            WaterLevelY += ((WaterLevel/WaterMax* WaterVisualSize) + transform.position.y + WaterVisualOffset);
        }
        timeSinceLastValue += Time.deltaTime;
        
        waterVisualsCounter++;
        if(timeSinceLastValue >= 1f) { 
            lastWaterLevelY = currentWaterLevelY;
            currentWaterLevelY = WaterLevelY / waterVisualsCounter;
            WaterLevelY = 0;
            timeSinceLastValue = 0;
            waterVisualsCounter = 0;
            if (WaterPercentage>=OutflowMinPercentage)
            {
                foreach (var item in OverflowVisual)
                {
                    item.SetActive(true);
                }
            }
            else
            {
                foreach (var item in OverflowVisual)
                {
                    item.SetActive(false);
                }
            }
        }
        waterLevel.transform.position = new Vector3(waterLevel.transform.position.x, Mathf.Lerp(lastWaterLevelY,currentWaterLevelY,timeSinceLastValue), waterLevel.transform.position.z);
    }
    void OutPutFlow()
    {
        foreach (GameObject outPut in OutPuts)
        {
            WaterScript outWS = outPut.GetComponent<WaterScript>();
            if (outWS.isBlocked == false && WaterPercentage > outWS.WaterPercentage + outWS.minimumFlowPercentage) { 
                WaterLevel -= OutFlowRate * Time.fixedDeltaTime;
                outWS.InPutFlow(OutFlowRate);
            }
        }
    }
    public void InPutFlow(float InFlow)
    {
        WaterLevel += InFlow * Time.fixedDeltaTime;
    }
    void Source()
    {
        WaterLevel += SourceFlow * Time.fixedDeltaTime;
    }
    void Leak()
    {
        if (WaterPercentage <= LeakLimitPercentage && isFixed == false)
        {
            LeakCleared.Invoke();
            isFixed = true;
        }
    }
    void Drain()
    {
        if (WaterPercentage >= DrainLimitPercentage) { 
            WaterLevel -= DrainFlow * Time.fixedDeltaTime;
        }
    }
    void BackFlow()
    {
        foreach(GameObject input in InPuts) { 
        if(input.GetComponent<WaterScript>().WaterPercentage + minimumFlowPercentage < WaterPercentage)
            {
            WaterLevel -= OutFlowRate * Time.fixedDeltaTime ;
            input.GetComponent<WaterScript>().InPutFlow(OutFlowRate);
            }
        }
    }
    public void doBlock()
    {
        isBlocked = true;
    }
    public void doUnBlock()
    {
        isBlocked = false;
    }
}
