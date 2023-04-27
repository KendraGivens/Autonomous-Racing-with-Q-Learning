using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//Allows camera to follow the car
public class CameraController : MonoBehaviour
{
    public Transform objectToFollow;
    public Rigidbody objectRB;
    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;
    public float lookHeightOffset = 2;
    public float threshold = 1;
    private float z_flip = 1;

    public void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        _lookDirection.y += lookHeightOffset;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }
     
    public void MoveToTarget()
    {
        if (objectToFollow.InverseTransformDirection(objectRB.velocity).z < -threshold)
        {
            z_flip = -1;
        }
        else if (objectToFollow.InverseTransformDirection(objectRB.velocity).z > threshold)
        {
            z_flip = 1;
        } 

        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z * z_flip +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }

}
