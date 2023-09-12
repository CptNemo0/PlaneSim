using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class New_AirplanePhisics : MonoBehaviour
{



    #region Variables
    [Header("KEY INPUTS")]
    public KeyCode IncreaseThrottle, DecreaseThrottle;
    public KeyCode AirBrakes_KEY;
    
    // Start is called before the first frame update
    [Space]
    [Header("COMPONENTS")]
    Rigidbody rig;


    [Header("FORCES")]
    [Space]
    [Header("VECTORS")]
    [SerializeField] public  Vector3 Velocity;
    [SerializeField] private Vector3 lastVelocity;
    [SerializeField] public Vector3 LocalVelocity;
    [SerializeField] public Vector3 LocalAngularVelocity;
    [SerializeField] public  Vector3 localGForce;
    [Space]
    [Header("DRAG")]
    [SerializeField] private AnimationCurve DragForward;
    [SerializeField] private AnimationCurve DragBack;
    [SerializeField] private AnimationCurve DragLeft;
    [SerializeField] private AnimationCurve DragRight;
    [SerializeField] private AnimationCurve DragTop;
    [SerializeField] private AnimationCurve DragBottom;
    [SerializeField] private AnimationCurve InducedDragCurve;
    [SerializeField] private AnimationCurve RudderInducedDragCurve;
    [SerializeField] Vector3 angularDrag;

    [SerializeField] private bool AirbrakeDeployed, FlapsDeployed;
    [SerializeField] private float airbrakeDrag, flapsDrag, inducedDrag, flapsLiftPower, flapsAOABias;
    [Space]
    [Header("LIFT")]
    [SerializeField] private AnimationCurve liftAOACurve;
    [SerializeField] private float liftPower;
    [SerializeField] private float rudderPower = 0;
    [SerializeField] private AnimationCurve rudderAOACurve;

    [Space]
    [Header("THRUST")]
    public float maxThrust=35;
    [Range(0, 1)]
    public float Throttle;

    [Space]
    [Header("THIS IS ANGLES")]
    //[SerializeField] private Vector3 LocalAngularVelocity;
    [SerializeField] public float AngleOfAttack;
    [SerializeField] public float AngleOfAttackYaw;

    [Space]
    [Header("STEERING")]
    [SerializeField] private Vector3 TurnSpeed = new Vector3(30, 15, 270);
    [SerializeField] private Vector3 TurnAcceleration = new Vector3(60, 30, 540);
    [SerializeField] private AnimationCurve SteeringCurve;
    [SerializeField] private float LerpInput = 23;
    [SerializeField] public Vector3 ControlInput;
    public Vector3 EffectiveInput { get; private set; }
    [Space]
    [Header("RANDOM MOVEMENTS")]
    [SerializeField] private float Intensity;
    [SerializeField] private float MinT, MaxT;
    public Transform Nose;
    [Header("BOUNCE")]
    [SerializeField] private Vector3 Bounce_;
    [Space]
    [SerializeField] private float bounceSpeed = 1.0f;
    [SerializeField] private float minBounce = -0.4f;
    [SerializeField] private float maxBounce = 0.4f;
    [Space]
    [Header("OVERSPEED")]
    [SerializeField] private float MaxSpeed = 50;
    [SerializeField] float gLimit;
    [SerializeField] float gLimitPitch;

    #endregion
    void Awake()
    {
        rig = this.GetComponent<Rigidbody>();
    }

    #region Thrust Class
    void UpdateThrust()
    {
        rig.AddRelativeForce(Throttle*maxThrust*Vector3.forward);
    }
    #endregion

   
    void FixedUpdate()
    {

        float dt = Time.fixedDeltaTime;
        #region Inputs

        float minMax = 1f;
        for (int i = 0; i < 3; i++) // Loop through x, y, and z
        {
           ControlInput[i] = Mathf.Clamp(ControlInput[i], -minMax, minMax);
        }

        Vector3 newBounce = new Vector3(-Input.GetAxis("Vertical"), Input.GetAxis("Yaw"), Input.GetAxis("Horizontal"));

        Bounce_ = Vector3.Lerp(Bounce_, newBounce, dt * bounceSpeed);
        if (Mathf.Approximately(newBounce.x, 0.0f)) newBounce.x += Random.Range(minBounce, maxBounce);
        if (Mathf.Approximately(newBounce.z, 0.0f)) newBounce.z += Random.Range(minBounce, maxBounce);
        if (Mathf.Approximately(newBounce.y, 0.0f)) newBounce.y += Random.Range(minBounce, maxBounce);




        ControlInput = Vector3.Lerp(ControlInput, new Vector3(-Input.GetAxis("Vertical"), Input.GetAxis("Yaw") + (-Input.GetAxis("Horizontal") / 10), Input.GetAxis("Horizontal") + (-Input.GetAxis("Yaw") / 10)), LerpInput * dt);


        if (Input.GetKey(IncreaseThrottle) && Throttle < 1) { Throttle += 0.5f * dt; } else if (Throttle > 1) { Throttle = 1; };
        if (Input.GetKey(DecreaseThrottle)&&Throttle>0) { Throttle -= 0.5f * dt; } else if (Throttle <0) { Throttle = 0; };
        if (Input.GetKey(AirBrakes_KEY)) { AirbrakeDeployed = true; } else { AirbrakeDeployed = false; }

        #endregion


        #region Classes

        CalculateState(dt);
        CalculateGForce(dt);
        UpdateThrust();
        UpdateLift();
        UpdateSteering(dt);
        UpdateDrag();
        UpdateAngularDrag();
        CalculateState(dt);
        InvokeRepeating("RandomMovements", Random.Range(MinT, MaxT), Random.Range(MinT, MaxT));
        LimitOverSpeed();

        #endregion







    }


    #region State Class
    void CalculateState(float dt)
    {
        var invRotation = Quaternion.Inverse(rig.rotation);
        Velocity = rig.velocity;
        LocalVelocity = invRotation * Velocity; 
        LocalAngularVelocity = invRotation * rig.angularVelocity;  

        CalculateAngleOfAttack();
    }
    #endregion


    #region Angle of Attack Class
    void CalculateAngleOfAttack()
    {
        if (LocalVelocity.sqrMagnitude < 0.1f)
        {
            AngleOfAttack = 0;
            AngleOfAttackYaw = 0;
            return;
        }

        AngleOfAttack = Mathf.Atan2(-LocalVelocity.y, LocalVelocity.z);
        AngleOfAttackYaw = Mathf.Atan2(LocalVelocity.x, LocalVelocity.z);
    }
    #endregion


    #region Lift Class
    void UpdateLift()
    {
        if (LocalVelocity.sqrMagnitude < 1f) return;

        float flapsLiftPower = FlapsDeployed ? this.flapsLiftPower : 0;
        float flapsAOABias = FlapsDeployed ? this.flapsAOABias : 0;

        var liftForce = CalculateLift(
            AngleOfAttack + (flapsAOABias * Mathf.Deg2Rad), Vector3.right,
            liftPower + flapsLiftPower,
            liftAOACurve,
            InducedDragCurve
        );

        var yawForce = CalculateLift(AngleOfAttackYaw, Vector3.up, rudderPower, rudderAOACurve, RudderInducedDragCurve);

        rig.AddRelativeForce(liftForce);
        rig.AddRelativeForce(yawForce);
    }


    Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis, float liftPower, AnimationCurve aoaCurve, AnimationCurve inducedDragCurve)
    {
        var liftVelocity = Vector3.ProjectOnPlane(LocalVelocity, rightAxis);    
        var v2 = liftVelocity.sqrMagnitude;                                     

        
        var liftCoefficient = aoaCurve.Evaluate(angleOfAttack * Mathf.Rad2Deg);
        var liftForce = v2 * liftCoefficient * liftPower;

        
        var liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        var lift = liftDirection * liftForce;

        
        var dragForce = liftCoefficient * liftCoefficient;
        var dragDirection = -liftVelocity.normalized;
        var inducedDrag = dragDirection * v2 * dragForce * this.inducedDrag * inducedDragCurve.Evaluate(Mathf.Max(0, LocalVelocity.z));

        return lift + inducedDrag;
    }

    #endregion

    #region Drag Class
    void UpdateAngularDrag()
    {
        var av = LocalAngularVelocity;
        var drag = av.sqrMagnitude * -av.normalized;   
        rig.AddRelativeTorque(Vector3.Scale(drag, angularDrag), ForceMode.Acceleration); 
    }
    void UpdateDrag()
    {
        var lv = LocalVelocity;
        var lv2 = lv.sqrMagnitude;  

        float airbrakeDrag = AirbrakeDeployed ? this.airbrakeDrag : 0;
        float flapsDrag = FlapsDeployed ? this.flapsDrag : 0;

       
        var coefficient = Scale6(
            lv.normalized,
            DragRight.Evaluate(Mathf.Abs(lv.x)), DragLeft.Evaluate(Mathf.Abs(lv.x)),
            DragTop.Evaluate(Mathf.Abs(lv.y)), DragBottom.Evaluate(Mathf.Abs(lv.y)),
            DragForward.Evaluate(Mathf.Abs(lv.z)) + airbrakeDrag + flapsDrag,   
            DragBack.Evaluate(Mathf.Abs(lv.z))
        );

        var drag = coefficient.magnitude * lv2 * -lv.normalized;    

        rig.AddRelativeForce(drag);
    }

    #endregion



    
    #region G force Class
    
    float CalculateGLimiter(Vector3 controlInput, Vector3 maxAngularVelocity)
    {
        if (controlInput.magnitude < 0.01f)
        {
            return 1;
        }

        
        var maxInput = controlInput.normalized;

        var limit = CalculateGForceLimit(maxInput);
        var maxGForce = CalculateGForce(Vector3.Scale(maxInput, maxAngularVelocity), LocalVelocity);

        if (maxGForce.magnitude > limit.magnitude)
        {
            
            return limit.magnitude / maxGForce.magnitude;
        }

        return 1;
    }
    Vector3 CalculateGForce(Vector3 angularVelocity, Vector3 velocity)
    {
        
        return Vector3.Cross(angularVelocity, velocity);
    }
    Vector3 CalculateGForceLimit(Vector3 input)
    {
        return Scale6(input,
            gLimit, gLimitPitch,    
            gLimit, gLimit,         
            gLimit, gLimit          
        ) * 9.81f;
    }
    void CalculateGForce(float dt)
    {
        var invRotation = Quaternion.Inverse(rig.rotation);
        var acceleration = (Velocity - lastVelocity) / dt;
        localGForce = invRotation * acceleration;
        lastVelocity = Velocity;
    }
    #endregion



    #region Static Scale6
    public static Vector3 Scale6(
       Vector3 value,
       float posX, float negX,
       float posY, float negY,
       float posZ, float negZ
   )
    {
        Vector3 result = value;

        if (result.x > 0)
        {
            result.x *= posX;
        }
        else if (result.x < 0)
        {
            result.x *= negX;
        }

        if (result.y > 0)
        {
            result.y *= posY;
        }
        else if (result.y < 0)
        {
            result.y *= negY;
        }

        if (result.z > 0)
        {
            result.z *= posZ;
        }
        else if (result.z < 0)
        {
            result.z *= negZ;
        }

        return result;
    }

    #endregion


    #region Steering


    void UpdateSteering(float dt)
    {
        var speed = Mathf.Max(0, LocalVelocity.z);
        var steeringPower = SteeringCurve.Evaluate(speed);

        var gForceScaling = CalculateGLimiter(ControlInput, TurnSpeed * Mathf.Deg2Rad * steeringPower);

        var targetAV = Vector3.Scale(ControlInput, TurnSpeed * steeringPower * gForceScaling);
        var av = LocalAngularVelocity * Mathf.Rad2Deg;

        var correction = new Vector3(
            CalculateSteering(dt, av.x, targetAV.x, TurnAcceleration.x * steeringPower),
            CalculateSteering(dt, av.y, targetAV.y, TurnAcceleration.y * steeringPower),
            CalculateSteering(dt, av.z, targetAV.z, TurnAcceleration.z * steeringPower)
        );

        rig.AddRelativeTorque(correction * Mathf.Deg2Rad, ForceMode.VelocityChange);    

        var correctionInput = new Vector3(
            Mathf.Clamp((targetAV.x - av.x) / TurnAcceleration.x, -1, 1),
            Mathf.Clamp((targetAV.y - av.y) / TurnAcceleration.y, -1, 1),
            Mathf.Clamp((targetAV.z - av.z) / TurnAcceleration.z, -1, 1)
        );

        var effectiveInput = (correctionInput + ControlInput) * gForceScaling;

        EffectiveInput = new Vector3(
            Mathf.Clamp(effectiveInput.x, -1, 1),
            Mathf.Clamp(effectiveInput.y, -1, 1),
            Mathf.Clamp(effectiveInput.z, -1, 1)
        );
    }

   

    float CalculateSteering(float dt,float angularVelocity, float targetVelocity, float acceleration)
    {
        var error = targetVelocity - angularVelocity;
        var accel = acceleration * dt;
        return Mathf.Clamp(error, -accel, accel);

    }
    #endregion

    void LimitOverSpeed()
    {
        if (LocalVelocity.z > MaxSpeed&&MaxSpeed!=0)
        {
            rig.AddRelativeForce((maxThrust*LocalVelocity.z)* Vector3.back);
        }



    }


    void RandomMovements()
    {

        // WaitForSecondsRealtime t = new WaitForSecondsRealtime(Random.Range(0.2f, 2f));


        ControlInput += new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.2f, 0.2f), Random.Range(-0.7f, 0.7f)) * Intensity;
    }




}
