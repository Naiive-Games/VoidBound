using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
        Destroy(gameObject);
        
    }
}
