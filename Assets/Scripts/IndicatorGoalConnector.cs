using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorGoalConnector : MonoBehaviour
{
    //Public variables
    [SerializeField] public Indicator indicator;
    [SerializeField] public GoalManager goalManager;

    //Unity functions
    void Update()
    {
        if (!goalManager.Goals[0].GetComponent<Ring>().Active)
        {
            goalManager.Goals.RemoveAt(0);
            indicator.Goal = goalManager.Goals[0];
        }
    }
}
