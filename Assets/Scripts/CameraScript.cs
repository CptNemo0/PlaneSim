using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Public variables
    [SerializeField] Transform pov;
    [SerializeField] private Transform Parent;
    //[SerializeField] Transform Look;
    [SerializeField] private Vector3 Offset;
    [SerializeField] float Fspeed;
    public float Sensivility;
    public float maxXRotation = 90.0f;
    public float minXRotation = -90.0f;
    private float currentXRotation = 0.0f;
    private float currentYRotation = 0.0f;
    private void Start()
    {
        pov = GameObject.FindGameObjectWithTag("Player").transform;
        if (transform.parent)
        {
            Parent = transform.parent;
        }
        Cursor.lockState = CursorLockMode.Locked;

    }


    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

       

        if (Parent)
        {

            currentXRotation -= mouseY * Sensivility;
            currentYRotation += mouseX * Sensivility;
            currentXRotation = Mathf.Clamp(currentXRotation, minXRotation, maxXRotation);
            Vector3 rotation = new Vector3(currentXRotation, currentYRotation, 0);
            Parent.rotation = Quaternion.Euler(rotation);



            //rotation.z = 0;           
            
        }
    }


    //Unity functions
    void FixedUpdate()
    {
      
       
        Vector3 multiplier = pov.GetComponent<New_AirplanePhisics>().Velocity;

       
           
           
            Parent.position = Vector3.Lerp(Parent.position, pov.position, Time.deltaTime * Fspeed + multiplier.z);          
            transform.localPosition = Offset;

        
       
    }
}
