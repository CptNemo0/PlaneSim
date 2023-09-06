using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    //Private variables
    private bool active;

    //Properties
    public bool Active { get => active; set => active = value; }

    //Unity functions
    private void Awake()
    {
        active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log(transform.name);
            active = false;
        }
    }
}
