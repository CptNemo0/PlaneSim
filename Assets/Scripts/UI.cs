using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    //Public variables
    [SerializeField] public Airplane airplane;

    //Private variables
    private Label altitude;
    private Label velocity;
    private Label throttle;
    private Label angleoat;

    //My Functions
    private void UpdateAltitude()
    {
        altitude.text = "Altitude:        " + airplane.transform.position.y.ToString("F0") + " m";
    }

    private void UpdateVelocity() 
    {
        velocity.text = "Velocity:        " + airplane.Rb.velocity.magnitude.ToString("F0") + " m/s";
    }

    private void UpdateThrottle() 
    {
        throttle.text = "Throttle:        " + airplane.Throttle.ToString("F0") + "%";
    }

    private void UpdateAot()
    {
        float aot = Mathf.Abs(airplane.transform.eulerAngles.x);
        
        if(aot > 90.0f)
        {
            aot = Mathf.Abs(aot - 360.0f);
        }
        angleoat.text = "Angle of attack: " + aot.ToString("F0") + "°";
    }

    //Untity Functions
    private void Awake()
    {
        //VisualElement root = GetComponent<UIDocument>().rootVisualElement;



       // altitude = root.Q<Label>("Altitude");
       // velocity = root.Q<Label>("Velocity");
      // throttle = root.Q<Label>("Throttle");
       // angleoat = root.Q<Label>("Angle");
    }

    private void Update()
    {
        UpdateAltitude();
        UpdateVelocity();
        UpdateThrottle();
        UpdateAot();
    }
}
