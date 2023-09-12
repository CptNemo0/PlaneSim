using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] float discountFactor;
    [SerializeField] float limit;
    public Rigidbody rig;
    public bool decelerate;
    
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        decelerate = false;
    }


    void Update()
    {
        if (rig.velocity.magnitude > limit)
        {
            if (decelerate)
            {
                rig.velocity *= discountFactor;
                rig.angularVelocity *= discountFactor;
            }
        }
    }
}
