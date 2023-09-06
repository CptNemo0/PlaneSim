using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] public Indicator indicator;
    [SerializeField] public GoalManager goalManager;

    void Update()
    {
        if (!goalManager.Goals[0].GetComponent<Circle>().Active)
        {
            goalManager.Goals.RemoveAt(0);
            indicator.Goal = goalManager.Goals[0];
        }
    }
}
