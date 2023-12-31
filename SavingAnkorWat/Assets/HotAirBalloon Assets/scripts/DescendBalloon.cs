using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendBalloon : MonoBehaviour  //reversed the balloon movement to make exterior move towards balloon because lever spackte rum wenn balloon moved instead
{
    public BunsenBurnerLever LeverScript;

    public GameObject HotAirBalloon;
    private float speed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(HotAirBalloon.transform.position, this.transform.position) < 50)
        {
            LeverScript.reachGoal = true;
            
            this.transform.position = Vector3.MoveTowards(this.transform.position,HotAirBalloon.transform.position,speed * Time.deltaTime);

        }
        else
        {
            this.transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
        }
    }
}
