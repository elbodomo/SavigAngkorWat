using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    
    public float WaterLevel = 0;
    [SerializeField] float WaterMax = 30;
    float WaterPercentage;
    [SerializeField] float WaterOutMin = 10;
    [SerializeField] float SourceFlow = 3f;
    [SerializeField] bool isSource = false;
    [SerializeField] float DrainFlow = 2f;
    [SerializeField] bool   isDrain = false;
    [SerializeField] bool isBlocked = false;
    [SerializeField] GameObject waterLevel;
    public GameObject InPut;
    public GameObject blockade;
    float OutFlowRate = 30f;
    float timeSinceLastValue = 0f;
    int waterVisualsCounter = 0;
    float WaterLevelY;
    float lastWaterLevelY = 0;
    float currentWaterLevelY = 0;
    [SerializeField] List<GameObject> OutPuts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        WaterLevelY = waterLevel.transform.position.y;
        foreach(GameObject outPut in OutPuts)
        {
            outPut.GetComponent<WaterScript>().InPut = this.gameObject;
        } 
    }
    // Update is called once per frame
    void Update()
    {
        if(isBlocked && blockade.activeSelf == false)
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
        if (WaterLevel >= WaterMax)
        {
            WaterLevel = WaterMax;
            OverFlow();
        }
        if (InPut != null && InPut.GetComponent<WaterScript>().WaterPercentage <= WaterPercentage && isBlocked==false)
        {
            BackFlow();
        }

        WaterPercentage = WaterLevel/WaterMax*100; 
        UpdateWaterLevel();
        
        OutPutFlow();
    }
    void UpdateWaterLevel()
    {
        if (isSource)
        {
            WaterLevelY += ((OutPuts[0].GetComponent<WaterScript>().WaterLevel / OutPuts[0].GetComponent<WaterScript>().WaterMax * 0.1f) + 0.05f);
        }
        else { 
            WaterLevelY += ((WaterLevel/WaterMax*0.1f)+0.05f);
        }
        timeSinceLastValue += Time.deltaTime;
        
        waterVisualsCounter++;
        if(timeSinceLastValue >= 1) { 
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
            if (!outWS.isBlocked && WaterPercentage > outWS.WaterPercentage) { 
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
    void Drain()
    {
        if (WaterPercentage >= 30f) { 
            WaterLevel -= DrainFlow * Time.deltaTime;
        }
    }
    void BackFlow()
    {
        WaterLevel -= OutFlowRate * Time.deltaTime;
        InPut.GetComponent<WaterScript>().InPutFlow(OutFlowRate);
    }
    
    void OverFlow()
    {


    }
}
