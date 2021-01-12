using UnityEngine;
using System.Collections;

public class CarCtrl: MonoBehaviour {

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;
    public Transform WheelFLtrans;
    public Transform WheelFRtrans;
    public Transform WheelRLtrans;
    public Transform WheelRRtrans;
    
    public AudioClip AC;
    public Vector3 com;
    float gravity = 9.8f;
    private bool braked = false;
    private float maxBrakeTorque = 500;
    private Rigidbody rb;
    private float maxTorque = 1000;

    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com; // Set the car's center of gravity
    }

    // collision sound
    public void OnCollisionEnter(Collision collision)   
    {
        //The origin of the hit object makes a sound (the second parameter is used to set the world coordinates of the sound)  
        AudioSource.PlayClipAtPoint(AC, transform.localPosition);  
    } 

    void FixedUpdate () {
        if(!braked){
            WheelFL.brakeTorque = 0;
            WheelFR.brakeTorque = 0;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
        }
        // Car forward torque
        WheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical");
        WheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical");

        // Car rotation angle
        WheelFL.steerAngle = 30 * (Input.GetAxis("Horizontal"));
        WheelFR.steerAngle = 30 * Input.GetAxis("Horizontal");

    }
    
    void Update()
    {
        Brake(); // brake

        // Wheel rotation
        WheelFLtrans.Rotate(WheelFL.rpm/60*360*Time.deltaTime ,0,0);
        WheelFRtrans.Rotate(WheelFR.rpm/60*360*Time.deltaTime ,0,0);
        WheelRLtrans.Rotate(WheelRL.rpm/60*360*Time.deltaTime ,0,0);
        WheelRRtrans.Rotate(WheelRL.rpm/60*360*Time.deltaTime ,0,0);

        // Wheels change direction
        Vector3 localeuler = WheelFLtrans.localEulerAngles;
        Vector3 localeuler2 = WheelFRtrans.localEulerAngles;
        localeuler.y = WheelFL.steerAngle;
        WheelFLtrans.localEulerAngles = localeuler;
        localeuler2.y = WheelFR.steerAngle;
        WheelFRtrans.localEulerAngles = localeuler2;
    }

    // brake
    void Brake()
    {
        if(Input.GetButton("Jump"))
        {
            braked = true;
        }
        else
        {
            braked = false;
        }
        if(braked){
         
            WheelRL.brakeTorque = maxBrakeTorque * 20;
            WheelRR.brakeTorque = maxBrakeTorque * 20;
            WheelRL.motorTorque = 0;
            WheelRR.motorTorque = 0;
        }
    }
}