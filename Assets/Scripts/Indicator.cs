using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] public GameObject dummyGoal; //used only to test if indicator works properly

    //Public variables
    private GameObject goal;                      

    //Properties
    public GameObject Goal { get => goal; set => goal = value; } 

    //Unity functions
    void Update()
    {
        if (goal is not null)
        {
            transform.rotation = Quaternion.LookRotation(Goal.transform.position - transform.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(dummyGoal.transform.position - transform.position);
        }
    }
}
