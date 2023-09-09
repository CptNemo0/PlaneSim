//using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    #region Variables
    [Header("Game objects")]
    [SerializeField] public GameObject ringPrefab;
    [SerializeField] public Indicator indicator;

    [Space]
    [Header("Amount of rings")]
    [SerializeField] public int length;

    [Space]
    [Header("Position of first ring")]
    [SerializeField] public int startX;
    [SerializeField] public int startY;
    [SerializeField] public int startZ;

    [Space]
    [Header("Position variables")]
    [SerializeField] public int deltaX;
    [SerializeField] public int deltaY;

    [Space]
    [Header("Distance variables")]
    [SerializeField] public int minDistance;
    [SerializeField] public int maxDistance;
    #endregion

    #region Lists
    private List<GameObject> goals = new List<GameObject>();
    private List<Vector3> points = new List<Vector3>();
    #endregion

    #region Properties
    public List<GameObject> Goals { get => goals; set => goals = value; }
    #endregion 

    
    private Vector3 GeneratePoint(Vector3 previous)
    {
        Vector3 newPoint = new Vector3();

        newPoint.x = previous.x + Random.Range(-deltaX, deltaX);
        newPoint.y = previous.y + Random.Range(-deltaY, deltaY);
        newPoint.z = previous.z + Random.Range(minDistance, maxDistance);

        return newPoint;
    }

    private void GeneratePoints()
    {
        Vector3 initial = new Vector3(startX, startY, startZ);
        points.Add(initial);

        for(int i = 1; i < length; i++)
        {
            points.Add(GeneratePoint(points[i - 1]));
        }
    }

    private void InstantiateGolas()
    {
        for(int i = 0; i < points.Count; i++) 
        {
            GameObject gameObject = Instantiate(ringPrefab);
            gameObject.transform.name = i.ToString();
            gameObject.transform.position = points[i];
            if(i > 0)
            {
                gameObject.transform.LookAt(points[i - 1]);
            }
            Goals.Add(gameObject);
        }
    }

    //Unity functions    
    private void Awake()
    {
        GeneratePoints();
        InstantiateGolas();
        indicator.Goal = Goals[0];
    }

}
