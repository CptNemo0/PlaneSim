using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorGoalConnector : MonoBehaviour
{
    #region Variables
    [Header("Game objects")]
    [SerializeField] public Indicator indicator;
    [SerializeField] public GoalManager goalManager;
    #endregion

    void Update()
    {
        if (!goalManager.Goals[0].GetComponent<Ring>().Active)
        {
            GameObject go = goalManager.Goals[0];
            goalManager.Goals.RemoveAt(0);
            Destroy(go);
            indicator.Goal = goalManager.Goals[0];
        }
    }
}
