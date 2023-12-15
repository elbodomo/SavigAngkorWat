using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaterScript : MonoBehaviour
{
    
    public float WaterLevel = 0;
    [SerializeField] private float WaterMax = 30;
    private float WaterPercentage = 1;
    private float minimumFlowPercentage = 3;
    [SerializeField] private float WaterOutMin = 10;
    [SerializeField] private float SourceFlow = 3f;
    [SerializeField] private bool isSource = false;
    [SerializeField] private float DrainFlow = 2f;
    [SerializeField] private bool isDrain = false;
    [SerializeField] private float LeakLimitPercentage = 15f; 
    [SerializeField] private bool isLeak = false;
    [SerializeField] private bool isBlocked = false;
    [SerializeField] private GameObject waterLevel;

    [SerializeField] private UnityEvent LeakCleared;
    public bool isFixed = false;
    public GameObject blockade;
    float OutFlowRate = 30f;
    float timeSinceLastValue = 0f;
    int waterVisualsCounter = 0;
    float WaterLevelY;
    float lastWaterLevelY = 0;
    float currentWaterLevelY = 0;

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
    // Update is called once per frame
    void Update()
    {

        WaterPercentage = WaterLevel / WaterMax * 100;
        Debug.Log(WaterPercentage);
        if (isBlocked && blockade.activeSelf == false)
        {
            blockade.SetActive(true);
        }
        if (isBlocked == false && blockade.activeSelf )
        {
            blockade.SetActive(false);
        }

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
        if (InPuts != null && isBlocked==false)
        {
            BackFlow();
        }
        OutPutFlow();
        UpdateWaterLevel();
        
    }
    void UpdateWaterLevel()
    {
        if (isSource)
        {
            WaterLevelY += ((OutPuts[0].GetComponent<WaterScript>().WaterLevel / OutPuts[0].GetComponent<WaterScript>().WaterMax * 0.3f) + transform.position.y + 0.15f);
        }
        else { 
            WaterLevelY += ((WaterLevel/WaterMax*0.3f) + transform.position.y + 0.15f);
        }
        timeSinceLastValue += Time.deltaTime;
        
        waterVisualsCounter++;
        if(timeSinceLastValue >= 0.2f) { 
            lastWaterLevelY = currentWaterLevelY;
            currentWaterLevelY = WaterLevelY / waterVisualsCounter;
            WaterLevelY = 0;
            timeSinceLastValue = 0;
            waterVisualsCounter = 0;
        }
        waterLevel.transform.position = new Vector3(waterLevel.transform.position.x, Mathf.Lerp(lastWaterLevelY,currentWaterLevelY,timeSinceLastValue), waterLevel.transform.position.z);
    }
    void OutPutFlow()
    {
        foreach (GameObject outPut in OutPuts)
        {
            WaterScript outWS = outPut.GetComponent<WaterScript>();
            if (!outWS.isBlocked && WaterPercentage > outWS.WaterPercentage + minimumFlowPercentage) { 
                WaterLevel -= OutFlowRate * Time.deltaTime;
                outWS.InPutFlow(OutFlowRate);
            }
        }
    }
    public void InPutFlow(float InFlow)
    {
        WaterLevel += InFlow * Time.deltaTime;
    }
    void Source()
    {
        WaterLevel += SourceFlow * Time.deltaTime;
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
        if (WaterPercentage >= 10f) { 
            WaterLevel -= DrainFlow * Time.deltaTime;
        }
    }
    void BackFlow()
    {
        foreach(GameObject input in InPuts) { 
        if(input.GetComponent<WaterScript>().WaterPercentage + minimumFlowPercentage < WaterPercentage)
            {
            WaterLevel -= OutFlowRate * Time.deltaTime;
            input.GetComponent<WaterScript>().InPutFlow(OutFlowRate * Time.deltaTime);
            }
        }
    }
}
