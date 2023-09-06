using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //Public variables
    private GameObject goal;

    //Properties
    public GameObject Goal { get => goal; set => goal = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Goal.transform.position - transform.position);
        //Debug.Log();
    }
}
