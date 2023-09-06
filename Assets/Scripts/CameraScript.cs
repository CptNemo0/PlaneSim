using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Public variables
    [SerializeField] Transform pov;
    [SerializeField] float speed;

    //Unity functions
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, pov.position, Time.deltaTime * speed);   
        transform.forward = pov.forward;
    }
}
