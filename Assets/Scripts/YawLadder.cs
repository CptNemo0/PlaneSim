using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YawLadder : MonoBehaviour
{
    [SerializeField] public GameObject barYawPrefab;
    [SerializeField] public int barInterval;
    [SerializeField] public int range;
    [SerializeField] public TextMeshProUGUI Deg_Text;
    

    struct Bar
    {
        public RectTransform transform;
        public float angle;
        public YawBar bar;

        public Bar(RectTransform transform, float angle, YawBar bar)
        {
            this.transform = transform;
            this.angle = angle;
            this.bar = bar;
        }
    }

    new RectTransform transform;
    List<Bar> bars;
    new Camera camera;
    Transform planeTransform;
    void Start()
    {
        

        transform = GetComponent<RectTransform>();
        bars = new List<Bar>();
       

        for (int i = 0; i < range; i++)
        {
            if (i % barInterval != 0) continue;

           
                CreateBar(i, barYawPrefab);
            
        }
    }
    public void SetCamera(Camera camera)
    {
        this.camera = Camera.main;
    }

    public void SetPlane(New_AirplanePhisics plane)
    {
        planeTransform = plane.GetComponent<Transform>();
    }

    void CreateBar(int angle, GameObject prefab)
    {
        var barGO = Instantiate(prefab, transform);
        var barTransform = barGO.GetComponent<RectTransform>();
        var bar = barGO.GetComponent<YawBar>();
        

        switch (angle)
        {
            case 0:
                bar.SetText(angle, "N");
                break;
            case 45:
                bar.SetText(angle, "NE");
                break;
            case 90:
                bar.SetText(angle, "E");
                break;
            case 135:
                bar.SetText(angle, "SE");
                break;
            case 180:
                bar.SetText(angle, "S");
                break;
            case 225:
                bar.SetText(angle, "SW");
                break;
            case 270:
                bar.SetText(angle, "W");
                break;
            case 315:
                bar.SetText(angle, "NW");
                break;
            default:
                //bar.SetText(angle, string.Empty);
                bar.SetText(0, string.Empty);
                
                break;
        }
        
        bars.Add(new Bar(barTransform, angle, bar));
    }

    float ConvertAngle(float angle)
    {
        //convert 0 - 360 range to -90 - 90
        if (angle > 90)
        {
            angle = Mathf.Abs(angle - 360);
        }
       
        return angle;
       
        //return angle > 180 ? angle - 360 : angle;
    }
    float GetPosition(float angle)
    {
        float fov = camera.fieldOfView;

        return TransformAngle(angle, fov, camera.pixelHeight);
    }
    public static float TransformAngle(float angle, float fov, float pixelHeight)
    {
        return (Mathf.Tan(angle * Mathf.Deg2Rad) / Mathf.Tan(fov / 2 * Mathf.Deg2Rad)) * pixelHeight / 2;
    }

    
    private void LateUpdate()
    {
        this.camera = Camera.main;
        if (camera == null) return;
        
        float roll = planeTransform.eulerAngles.z;
        float yaw = planeTransform.eulerAngles.y;

        //transform.localEulerAngles = new Vector3(0, 0, roll);

        foreach(var bar in bars) 
        {
            float angle = Mathf.DeltaAngle(yaw, bar.angle);
            float position = GetPosition(ConvertAngle(angle));
            Deg_Text.text = string.Format("{0:F0}�",yaw);
            if (Mathf.Abs(angle) < 90f && position >= transform.rect.xMin && position <= transform.rect.xMax)
            {
                //if bar position is within bounds
                var pos = bar.transform.localPosition;
                bar.transform.localPosition = new Vector3(position, pos.y, pos.z);
                bar.transform.gameObject.SetActive(true);
                
                if (bar.bar != null)
                {
                    bar.bar.UpdateRoll(roll);
                }
            }
            else
            {
                bar.transform.gameObject.SetActive(false);
            }
        }
    }
}
