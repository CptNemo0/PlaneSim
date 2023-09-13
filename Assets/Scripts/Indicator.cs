using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    #region Variables

    [Header("No rotate distance")]
    [SerializeField] public int noRotateDistance;
    [SerializeField]private GameObject goal;

    #endregion

    #region Properties
    [SerializeField] public GameObject Goal { get => goal; set => goal = value; }
    #endregion

    
    private void Update()
    {
        if (!(goal is null))
        {
            if(Vector3.Distance(Goal.transform.position, transform.position) > noRotateDistance) 
            {
                transform.LookAt(Goal.transform.position);  
            }
        }
        else
        {
            transform.LookAt(Vector3.zero);
        }
    }
}
