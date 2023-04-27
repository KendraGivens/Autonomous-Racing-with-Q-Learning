using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

//Controls the car
//Gathers the state information into a list
//Passes state information to player script
public class CarController2 : MonoBehaviour
{
    public float m_horizontalInput;
    public float m_verticalInput;

    private float m_steeringAngle;

    public Rigidbody car;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float steerSpeed = 10;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    public float offset;

    private Ray fl_ray, fr_ray, l_ray, r_ray, rl_ray, rr_ray;
    private RaycastHit fl_hit, fr_hit, l_hit, r_hit, rl_hit, rr_hit;
    public float hit_distance = 1000f;

    bool end_episode = false;
    public float collision_reward = 0f;
    public float checkpoint_reward = 0f;
    public float checkpoint_distance_reward = 0f;

    static bool isPaused = false;
    public static GameObject[] CheckPointsList;
    
    private int current_checkpoint = 0;
    private int num_checkpoints = 0;

    void OnCollisionEnter(Collision other)
    {
        end_episode = true;
        collision_reward = -5;
    }

    void OnCollisionExit(Collision other)
    {
        collision_reward = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Checkpoint" + current_checkpoint.ToString())
        { 
            if (current_checkpoint < num_checkpoints-1)
            {
                current_checkpoint++;
                checkpoint_reward = 5;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        checkpoint_reward = 0;
    }


    public List<float> GetState()
    {
        Vector3 velocity = car.transform.InverseTransformDirection(car.velocity);
        Vector3 distance = car.transform.position - CheckPointsList[current_checkpoint].transform.position;

        //Pass in current steering angle, speed, time it is colliding with wall, time around track, ray cast hit distances
        List<float> state = new List<float> {m_steeringAngle, velocity.x, velocity.z, fl_hit.distance, 
                                            fr_hit.distance, l_hit.distance, r_hit.distance, rl_hit.distance, rr_hit.distance, distance.x, distance.y, distance.z};
        return state;
   
    }

    private void Steer()
    {
        float steerDelta = 0;

        if (m_horizontalInput != 0)
        {
            steerDelta = steerSpeed * Time.fixedDeltaTime * m_horizontalInput;
            m_steeringAngle = Math.Min(Math.Max(m_steeringAngle + steerDelta, -maxSteerAngle), maxSteerAngle);
        }
        else
        {
            steerDelta = steerSpeed * Time.fixedDeltaTime * -Math.Sign(m_steeringAngle);
            if (Math.Abs(steerDelta) > Math.Abs(m_steeringAngle))
            {
                m_steeringAngle = 0;
            }
            else
            {
                m_steeringAngle += steerDelta;
            }

        }

        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;

    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void Rays()
    {
        Vector3 ray_position = transform.position;

        fl_ray = new Ray(ray_position - offset * transform.right, transform.forward); //Left forward
        fr_ray = new Ray(ray_position + offset * transform.right, transform.forward);//Right forward

        l_ray = new Ray(ray_position, -transform.right);   //Left side
        r_ray = new Ray(ray_position, transform.right);    //Right side
      
        rl_ray = new Ray(ray_position - offset * transform.right, -transform.forward);//Left reverse
        rr_ray = new Ray(ray_position + offset * transform.right, -transform.forward); //Right reverse
        
        //Forward Left
        if (Physics.Raycast(fl_ray, out fl_hit, hit_distance))
        {   
            UnityEngine.Debug.DrawRay(fl_ray.origin, fl_ray.direction * hit_distance, Color.yellow);
        }
        //Forward Right
        if (Physics.Raycast(fr_ray, out fr_hit, hit_distance))
        {
            UnityEngine.Debug.DrawRay(fr_ray.origin, fr_ray.direction * hit_distance, Color.yellow);
        }
        //Side Left 
        if (Physics.Raycast(l_ray, out l_hit, hit_distance))
        {
            UnityEngine.Debug.DrawRay(l_ray.origin, l_ray.direction * hit_distance, Color.yellow);
        }
        //Side Right
        if (Physics.Raycast(r_ray, out r_hit, hit_distance))
        {
            UnityEngine.Debug.DrawRay(r_ray.origin, r_ray.direction * hit_distance, Color.yellow);
        }
        //Reverse Left
        if (Physics.Raycast(rl_ray, out rl_hit, hit_distance))
        {
            UnityEngine.Debug.DrawRay(rl_ray.origin, rl_ray.direction * hit_distance, Color.yellow);
        }
        //Reverse Right
        if (Physics.Raycast(rr_ray, out rr_hit, hit_distance))
        {
            UnityEngine.Debug.DrawRay(rr_ray.origin, rr_ray.direction * hit_distance, Color.yellow);
        }
        
    }

    void Awake()
    {
        CheckPointsList = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var item in CheckPointsList)
        {
            num_checkpoints++;
        }
    }

    private void FixedUpdate()
    {
        Rays();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }
}