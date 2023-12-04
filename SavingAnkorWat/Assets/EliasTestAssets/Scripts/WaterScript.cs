using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public float WaterLevel = 0;
    [SerializeField] float WaterMax = 30;
    [SerializeField] float WaterOutMin = 10;
    [SerializeField] bool isSource = false;
    [SerializeField] GameObject waterLevel;

    [SerializeField] List<GameObject> OutPuts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (WaterLevel >= WaterMax)
        {
            WaterLevel = WaterMax;
            OverFlow();
        }

        UpdateWaterLevel();
        OutPut();
    }
    void UpdateWaterLevel()
    {

        float WaterLevelY = WaterLevel/WaterMax*0.1f;
        waterLevel.transform.position = new Vector3(waterLevel.transform.position.x, WaterLevelY , waterLevel.transform.position.z); 
    }
    void OutPut()
    {
        foreach (GameObject outPut in OutPuts)
        {


        }
    }
    void OverFlow()
    {


    }
}
