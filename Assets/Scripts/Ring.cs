using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    //Private variables
    #region Vairables
    private bool active;
    #endregion

    #region Properties
    public bool Active { get => active; set => active = value; }
    #endregion
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
