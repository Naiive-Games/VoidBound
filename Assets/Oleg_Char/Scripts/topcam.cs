using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TopDownCamera : MonoBehaviour
{
    public Transform target; 
    public float height = 10.0f; 
    public float z = 10.0f;
    public float x = 0f;

    void LateUpdate()
    {
        
        transform.position = target.position + new Vector3(x, height, z);
        
        transform.LookAt(target);
    }
}

