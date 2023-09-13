using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class New_HUD : MonoBehaviour
{
    [SerializeField] private Transform Plane;
    [SerializeField] private Transform CameraT;
    [SerializeField] private Camera cam;
    [SerializeField] Transform hudCenter;
    [SerializeField] Transform velocityMark;
    [SerializeField] PitchLadderBeh pitchLadder;
    [SerializeField] YawLadder yawLadder;
    New_AirplanePhisics plane;
    [Space]
    [SerializeField] private Image ThrottleUI;
    [SerializeField] TextMeshProUGUI Throttle_Text;
    [Space]  
    [SerializeField] TextMeshProUGUI AirSpeed_Text;
    [SerializeField] TextMeshProUGUI Altitude_Text;
    [Space]
    [Header("Lock ON")]
    [SerializeField] private GoalManager Goal_Manager;
    [SerializeField] private Transform TargetB;
    [SerializeField] private Transform Goal;
    // Start is called before the first frame update
    void Awake()
    {
        this.CameraT = Camera.main.transform;
        this.cam = Camera.main;
        this.Plane = GameObject.FindGameObjectWithTag("Player").transform;
        SetPlane(Plane.GetComponent<New_AirplanePhisics>());
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam == null) return;

        


        UpdateHUDCenter();
        UpdateVelocityMarker();


        if (Goal_Manager)
        {
            if (Goal_Manager.Goals.Count > 0)
            {
                Goal = Goal_Manager.Goals[0].gameObject.GetComponent<Transform>();
            }
            else
            {
                return;
            }
        }
        else if (GameObject.FindGameObjectWithTag("GoalManager").GetComponent<GoalManager>())
        {
            Goal_Manager = GameObject.FindGameObjectWithTag("GoalManager").GetComponent<GoalManager>();
        }
        else
        {

            return;
        }   



            
        
        
       
    }

     void Update()
    {
        GoalsHUD();
        
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
            this.Plane = plane.GetComponent<Transform>();
        }

       

        if (pitchLadder != null)
        {
            pitchLadder.SetPlane(plane);
            pitchLadder.SetCamera(cam);
        }

        if (yawLadder != null)
        {
            yawLadder.SetPlane(plane);
            yawLadder.SetCamera(cam);
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

        ThrottleUI.fillAmount = plane.Throttle;
        Throttle_Text.text = (Mathf.RoundToInt( plane.Throttle * 100)).ToString()+"%";
        AirSpeed_Text.text = (Mathf.RoundToInt(plane.LocalVelocity.z)).ToString();
        Altitude_Text.text = ((int)plane.transform.position.y).ToString();
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


    void GoalsHUD()
    {

        if (Goal)
        {         
            TargetB.position = cam.WorldToScreenPoint(Goal.position);

            var hudPos = TransformToHUDSpace(Goal.position);

            if (hudPos.z>0)
            {
                TargetB.gameObject.SetActive(true);
            }
            else
            {
                TargetB.gameObject.SetActive(false);
            }
       }           
        else
        {
            return;
         }

    }



    bool Visible(Camera c, Transform target)
    {

        var planes_ = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.position;


        foreach(var plane_ in planes_)
        {
            if (plane_.GetDistanceToPoint(point) < 0)
            {
                return false;
            }

        }
        return true;

    }


}
