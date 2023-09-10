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
    private float speeds;
    [SerializeField] private float rayDist;
    [SerializeField] private Transform LOOKAT;
    [Space]
    [SerializeField] private float X, Y;
    
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

       

        

            currentXRotation -= mouseY * Sensivility;
            currentYRotation += mouseX * Sensivility;
            currentXRotation = Mathf.Clamp(currentXRotation, minXRotation, maxXRotation);
            Vector3 rotation = new Vector3(currentXRotation, currentYRotation, 0);
            Parent.rotation = Quaternion.Euler(rotation);
            
        Vector2 rot = new Vector2(mouseX, mouseY);



       
           
        

       


        //rotation.z = 0;           


        LOOKAT.LookAt(pov.GetComponent<New_AirplanePhisics>().Nose.position);
        X = LOOKAT.localEulerAngles.x;
        Y = LOOKAT.localEulerAngles.y;
        Debug.DrawLine(LOOKAT.position, pov.GetComponent<New_AirplanePhisics>().Nose.position, Color.green);
        currentXRotation = Mathf.LerpAngle(currentXRotation,X , (Fspeed/7) * Time.deltaTime);
        currentYRotation = Mathf.LerpAngle(currentYRotation,Y , (Fspeed/7 )* Time.deltaTime);
        LOOKAT.position = Parent.position;

         
        transform.localPosition = Offset;
        Parent.position = pov.localPosition;

    }


    //Unity functions
    void FixedUpdate()
    {

       

        //Parent.position = Vector3.MoveTowards(Parent.position, pov.localPosition, 500* pov.GetComponent<New_AirplanePhisics>().LocalVelocity.z);

       
        

       
    }



    
}
