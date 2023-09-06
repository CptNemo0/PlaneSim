using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    private bool active;

    public bool Active { get => active; set => active = value; }

    private void Awake()
    {
        active = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            active = false;
        }
        GetComponent<BoxCollider>().enabled = false;
    }
}
