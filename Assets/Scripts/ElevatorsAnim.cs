using UnityEngine;
using UnityEngine.VFX.Utility;
using UnityEngine.VFX;

public class ElevatorsAnim : MonoBehaviour
{

    [SerializeField]private New_AirplanePhisics physics;


    [Space]
    [Header("ELEVATORS")]
    [SerializeField]private float ElevatorDeg;
    [SerializeField] private float E_max;
    [SerializeField] private float EL;
    [SerializeField] private Transform[] Elevators;
    [Space]
    [Header("FLAPS")]
    [SerializeField]private float FlapsDeg;
    [SerializeField] private float F_max;
    [SerializeField] private float FL;
     [SerializeField] private Transform[] Flaps;
    [Space]
    [SerializeField] private Transform Airbrake;
    [SerializeField] private float Airbrake_MaxOpen;
    [SerializeField] private float Airbrake_Deg;



    [Space]
    public ParticleSystem[] Gf_particles;
    [Space]
    [SerializeField] private VisualEffect[] ThrustVFX;
    [SerializeField] private Light[] AreaLights;
    [SerializeField] private float MinLumen, MaxLumen;

    [SerializeField] private float MinSize0, MaxSize0;
    [SerializeField] private float MinSize1, MaxSize1;
    [SerializeField] private float MinLength, MaxLength;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        ElevatorDeg = physics.ControlInput.x*E_max;
        EL = Mathf.Lerp(EL, ElevatorDeg, 25 * Time.deltaTime);

        Elevators[0].localRotation = Quaternion.Euler(EL, 0, 90);
        Elevators[1].localRotation = Quaternion.Euler(EL, 0, -90);



        FlapsDeg = physics.ControlInput.z * F_max;
        FL = Mathf.Lerp(FL, FlapsDeg, 25 * Time.deltaTime);

        Flaps[0].localRotation = Quaternion.Euler(-FL, 0, -90);
        Flaps[1].localRotation = Quaternion.Euler(FL, 0, 90);


        if (Input.GetKey(physics.AirBrakes_KEY)) { Airbrake_Deg = Mathf.Lerp(Airbrake_Deg, Airbrake_MaxOpen, 5 * Time.deltaTime); } else { Airbrake_Deg = Mathf.Lerp(Airbrake_Deg, 0, 40 * Time.deltaTime); }


        Airbrake.localRotation = Quaternion.Euler(Airbrake_Deg, 0, 0);

        foreach (ParticleSystem p in Gf_particles)
        {
            if (physics.localGForce.y > 15|| physics.localGForce.z < -5)
            {
                p.Play();
            }
            else { p.Stop(); 
            }
            
        }
        float Throttle = physics.Throttle;
        float realValue_0 = Mathf.Lerp(MinSize0, MaxSize0, Throttle);//Finds the real value for Size_0, from min to max depending on trottle position
        float realValue_1 = Mathf.Lerp(MinSize1, MaxSize1, Throttle);//Finds the real value for Size_1, from min to max depending on trottle position
        float realValueLength = Mathf.Lerp(MinLength, MaxLength, Throttle);//Finds the real value for Size_1, from min to max depending on trottle position


        foreach (VisualEffect vfx in ThrustVFX){
         vfx.SetFloat("Size_0", realValue_0);
            vfx.SetFloat("Size_1", realValue_1);
            vfx.SetFloat("Length", realValueLength);

            if (Throttle == 0)
            {
                //vfx.gameObject.SetActive(false);
            }
            else { vfx.gameObject.SetActive(!false); }
        }
       
 
        
        
        float realValueLumen = Mathf.Lerp(MinLumen, MaxLumen, Throttle);//Sets Min lumen when throttle at 0 and max when throttle at 100
        for (int i = 0; i < AreaLights.Length; i++)
        {

            
            AreaLights[i].intensity = realValueLumen;


        }

        /*
        foreach(Light Lights in AreaLights){

            float Throttle = physics.Throttle;
            float realValue_0 = Mathf.Lerp(MinLumen, MaxLumen, Throttle);//Sets Min lumen when throttle at 0 and max when throttle at 100
            Lights.intensity = realValue_0;





        }*/

    }
}
