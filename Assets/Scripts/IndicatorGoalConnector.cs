using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IndicatorGoalConnector : MonoBehaviour
{
    #region Variables
    [Header("Game objects")]
    [SerializeField] public Indicator indicator;
    [SerializeField] public GoalManager goalManager;
    private Stopwatch stopwatch;
    public static float elapsed;
    #endregion

    private void Awake()
    {
        elapsed = 0.0f;
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

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
                stopwatch.Stop();
                elapsed = (float)stopwatch.ElapsedMilliseconds / 1000.0f;
                SceneManager.LoadScene(3);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
