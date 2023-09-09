using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    #region Variables
    [SerializeField] public TextMeshProUGUI altitude;
    [SerializeField] public TextMeshProUGUI velocity;
    [SerializeField] public TextMeshProUGUI throttle;
    [SerializeField] public TextMeshProUGUI aoa;

    private New_AirplanePhisics airplane;
    #endregion

    private void UpdateTexts()
    {
        altitude.text = String.Format("Altitude: {0}m", (int)airplane.transform.position.y);
        velocity.text = String.Format("Velocity: {0}m/s", (int)airplane.LocalVelocity.z);
        throttle.text = String.Format("Throttle: {0}%", (int)airplane.Throttle);
        aoa.text = String.Format("AOA: {0}°", airplane.transform.eulerAngles.x);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.airplane = GameObject.FindGameObjectWithTag("Player").GetComponent<New_AirplanePhisics>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexts();
    }
}
