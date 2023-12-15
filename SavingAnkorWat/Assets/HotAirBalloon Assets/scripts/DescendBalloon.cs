using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendBalloon : MonoBehaviour
{
    public BunsenBurnerLever LeverScript;

    public GameObject destination;
    private float speed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        LeverScript = GetComponentInChildren<BunsenBurnerLever>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position, destination.transform.position) < 15)
        {
            LeverScript.reachGoal = true;
            
            this.transform.position = Vector3.MoveTowards(this.transform.position,destination.transform.position,speed * Time.deltaTime);

        }
        else
        {
            this.transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);
        }
    }
}
