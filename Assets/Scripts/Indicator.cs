using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //Public variables
    private GameObject goal;                      

    //Properties
    public GameObject Goal { get => goal; set => goal = value; } 

    //Unity functions
    void Update()
    {
        if (goal is not null)
        {
            float dist = Vector3.Distance(Goal.transform.position, transform.position);
            if(dist > 15) 
            {
                transform.rotation = Quaternion.LookRotation(Goal.transform.position - transform.position);
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 0.0f) - transform.position);
        }
    }
}
