using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneHUD : MonoBehaviour
{
    [SerializeField] private Transform Plane;
    [SerializeField] private Transform CameraT;
    [SerializeField] private Camera cam;
    [SerializeField] Transform hudCenter;
    [SerializeField] Transform velocityMark;
    [SerializeField] PitchLadderBeh pitchLadder;
    [SerializeField] YawLadder yawLadder;
    New_AirplanePhisics plane;


    

    // Start is called before the first frame update
    void Awake()
    {
        CameraT = Camera.main.transform;
        cam = Camera.main;
        Plane = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam == null) return;

        


        UpdateHUDCenter();
        UpdateVelocityMarker();
        SetPlane(Plane.GetComponent<New_AirplanePhisics>());
    }

    public void SetPlane(New_AirplanePhisics plane)
    {
        this.plane = plane;

        if (plane == null)
        {
            Plane = null;
        }
        else
        {
            Plane = plane.GetComponent<Transform>();
        }

       // if (compass != null)
       // {
         //   compass.SetPlane(plane);
       // }

        if (pitchLadder != null)
        {
            pitchLadder.SetPlane(plane);        
            pitchLadder.SetCamera(cam);
        }

        if (yawLadder != null)
        {
            yawLadder.SetPlane(plane);
        }
    }

    void UpdateHUDCenter()
    {
        var rotation = CameraT.localEulerAngles;
        var hudPos = TransformToHUDSpace(CameraT.position+Plane.forward);
        GameObject hudCenterGO = hudCenter.gameObject;
       
        if (hudPos.z > 0)
            {
            
                hudCenterGO.SetActive(true);
                hudCenter.localPosition = new Vector3(hudPos.x, hudPos.y, 0);
                hudCenter.localEulerAngles = new Vector3(0, 0, -rotation.z);
            }
            else
            {
                hudCenterGO.SetActive(false);
            }

        


    }

    void UpdateVelocityMarker()
    {

        New_AirplanePhisics plane = Plane.GetComponent<New_AirplanePhisics>();
        var velocity = Plane.forward;

        if (plane.LocalVelocity.sqrMagnitude > 1)
        {
            velocity = plane.GetComponent<Rigidbody>().velocity;
        }

        var hudPos = TransformToHUDSpace(CameraT.position + velocity);

        if (hudPos.z > 0)
        {
            velocityMark.gameObject.SetActive(true);
            velocityMark.localPosition = new Vector3(hudPos.x, hudPos.y, 0);
        }
        else
        {
            velocityMark.gameObject.SetActive(false);
        }
    }

    Vector3 TransformToHUDSpace(Vector3 worldSpace)
    {
        var screenSpace = cam.WorldToScreenPoint(worldSpace);
        return screenSpace - new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);
    }
}
