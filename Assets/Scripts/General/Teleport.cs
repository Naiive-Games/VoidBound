using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    
    public Transform pointstart;
    public Transform pointback;
    Vector3 targetPosition = new Vector3(pointback);
    public void OnTriggerEnter(Collider other) {
        
         other.transform.position = targetPosition;
        

    }

  


    
}


