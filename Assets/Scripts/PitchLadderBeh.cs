using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchLadderBeh : MonoBehaviour
{
    [SerializeField]
    GameObject pitchHorizonPrefab;
    [SerializeField]
    GameObject pitchPositivePrefab;
    [SerializeField]
    GameObject pitchNegativePrefab;
    [SerializeField]
    int barInterval;
    [SerializeField]
    int range;

    struct Bar
    {
        public RectTransform transform;
        public float angle;
        public PitchBar bar;

        public Bar(RectTransform transform, float angle, PitchBar bar)
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

        for (int i = -range; i <= range; i++)
        {
            if (i % barInterval != 0) continue;

            if (i == 0 || i == 90 || i == -90)
            {
                CreateBar(i, pitchHorizonPrefab);
            }
            else if (i > 0)
            {
                CreateBar(i, pitchPositivePrefab);
            }
            else
            {
                CreateBar(i, pitchNegativePrefab);
            }
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
        var bar = barGO.GetComponent<PitchBar>();
        int TextAngle = (angle + 180) % 360;





        if (TextAngle < 0)
        {
            if (bar != null) bar.SetNumber(TextAngle += 360);
        }
        
            if (bar != null) bar.SetNumber(TextAngle -= 180);
        

        

        bars.Add(new Bar(barTransform, angle, bar));
    }


    float ConvertAngle(float angle)
    {
        //convert 0 - 360 range to -180 - 180
        if (angle > 180)
        {
            angle -= 360f;
        }

        return angle;
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
    void LateUpdate()
    {
        this.camera = Camera.main;
        if (camera == null) return;

        //pitch == rotation around x axis
        //roll == rotation around z axis
        float pitch = -planeTransform.eulerAngles.x;
        float roll = planeTransform.eulerAngles.z;

        transform.localEulerAngles = new Vector3(0, 0, -roll);

        foreach (var bar in bars)
        {
            float angle = Mathf.DeltaAngle(pitch, bar.angle);
            float position = GetPosition(ConvertAngle(angle));

            if (Mathf.Abs(angle) < 90f && position >= transform.rect.yMin && position <= transform.rect.yMax)
            {
                //if bar position is within bounds
                var pos = bar.transform.localPosition;
                bar.transform.localPosition = new Vector3(pos.x, position, pos.z);
                bar.transform.gameObject.SetActive(true);

                if (bar.bar != null) bar.bar.UpdateRoll(roll);
            }
            else
            {
                bar.transform.gameObject.SetActive(false);
            }
        }
    }
}
