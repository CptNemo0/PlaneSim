using Palmmedia.ReportGenerator.Core.Reporting.History;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    //Public variables
    [SerializeField] public float maxThrust;
    [SerializeField] public float throttleDelta;
    [SerializeField] public float pitchDelta;
    [SerializeField] public float responsivenessRoll;
    [SerializeField] public float responsivenessPitch;
    [SerializeField] public float responsivenessYaw;
    [SerializeField] public float lift;
      
    //Private variables
    private float roll;             // angle between wings and ground, axis Z rotation
    private float pitch;            // angle of attack, axis X rotation
    private float yaw;              // turning, axis Y rotation
    private bool steering = false;

    //Private variables with properties    
    private float throttle;
    private Rigidbody rb;

    //Modifier
    private float RollModifier
    {
        get
        {
            return Rb.mass * responsivenessRoll;
        }
    }
    private float PitchModifier
    {
        get
        {
            return Rb.mass * responsivenessPitch;
        }
    }
    private float YawModifier
    {
        get
        {
            return Rb.mass * responsivenessYaw;
        }
    }

    //Properties
    public float Throttle { get => throttle; set => throttle = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    //My Functions
    private void HandleInput()
    {
        if(Input.mouseScrollDelta.y > 0) 
        {
            pitch -= pitchDelta;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            pitch += pitchDelta;
        }

        if(Input.GetButtonDown("Steering"))
        {
            steering = true;
        }

        if(Input.GetButtonUp("Steering"))
        {
            steering = false;    
        }

        roll = Input.GetAxis("Roll");

        if (steering)
        {
            Vector3 screenCentre = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
            Vector3 offset = screenCentre - Input.mousePosition;

            if(transform.up.y > 0.0f)
            {
                pitch = 1 * offset.y / (Screen.height * 0.25f);
            }
            else
            {
                pitch = -1 * offset.y / (Screen.height * 0.25f);
            }
            yaw = -1 * offset.x / (Screen.width * 0.25f);


        }
        
        if(Input.GetKey(KeyCode.W))
        {
            Throttle += throttleDelta;
        }
        else if(Input.GetKey(KeyCode.S)) 
        {
            Throttle -= throttleDelta;
        }

        pitch = Mathf.Clamp(pitch, -1.0f, 1.0f);
        yaw = Mathf.Clamp(yaw, -1.0f, 1.0f);
        roll = Mathf.Clamp(roll, -1.0f, 1.0f);
        Throttle = Mathf.Clamp(Throttle, 0.0f, 100.0f);
    }

    //Unity functions
    private void OnDrawGizmos()
    {
        if(steering) 
        {
            Vector3 screenCentre = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 10.0f);
            Vector3 pos = Input.mousePosition + new Vector3(0f, 0f, 10f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(Camera.main.transform.forward * 5 + Camera.main.ScreenToWorldPoint(screenCentre), Camera.main.transform.forward * 5 + Camera.main.ScreenToWorldPoint(pos));
        }

        Gizmos.color = Color.green;

    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Throttle = 0;
        roll = 0;
        pitch = 0;
        yaw = 0;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Rb.AddForce(transform.forward * maxThrust * Throttle);  //thrust
        Rb.AddTorque(-transform.forward * roll * RollModifier); //roll
        Rb.AddTorque(transform.right * pitch * PitchModifier);  //pitch
        Rb.AddTorque(transform.up * yaw * YawModifier);         //yaw
        Rb.AddForce(Vector3.up * Rb.velocity.magnitude * lift); //lift
    }
}
