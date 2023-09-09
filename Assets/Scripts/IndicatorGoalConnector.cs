using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (indicator.Goal.name != (goalManager.length - 1).ToString())
            {
                GameObject go = goalManager.Goals[0];
                goalManager.Goals.RemoveAt(0);
                Destroy(go);
                indicator.Goal = goalManager.Goals[0];
            }
            else
            {
                Time.timeScale = 0.0f;
                SceneManager.LoadScene(3);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
