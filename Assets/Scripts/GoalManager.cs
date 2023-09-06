using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    //Public variable
    [SerializeField] public GameObject goalPrefab;
    [SerializeField] public Indicator indicator;

    [SerializeField] public int length;

    [SerializeField] public int startX;
    [SerializeField] public int startY;
    [SerializeField] public int startZ;

    [SerializeField] public int deltaX;
    [SerializeField] public int deltaY;

    [SerializeField] public int minDistance;
    [SerializeField] public int maxDistance;

    //Lists
    private List<GameObject> goals = new List<GameObject>();
    private List<Vector3> points = new List<Vector3>();

    public List<GameObject> Goals { get => goals; set => goals = value; }

    //My functions
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
            points.Add(GeneratePoint(points[i-1]));
        }
    }

    private void InstantiateGolas()
    {
        for(int i = 0; i < points.Count; i++) 
        {
            GameObject gameObject = Instantiate(goalPrefab);
            gameObject.transform.name = i.ToString();
            gameObject.transform.position = points[i];
            if(i > 0)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - points[i - 1]);
            }
            Goals.Add(gameObject);
            
        }
    }


    //Unity functions    
    void Start()
    {
        GeneratePoints();
        InstantiateGolas();
        indicator.Goal = Goals[0];
    }

    void Update()
    {
        
    }
}
