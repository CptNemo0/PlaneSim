using UnityEngine;

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
            if (physics.localGForce.y > 10|| physics.localGForce.z < -4)
            {
                p.Play();
            }
            else { p.Stop(); }

        }
        





    }
}
