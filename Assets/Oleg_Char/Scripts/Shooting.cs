using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; // The prefab for the bullet
    public Transform firePoint; // The point from which the bullet will be fired

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add force to the bullet in the direction of the fire point
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 1000);
        
        // Destroy the bullet after 5 seconds
        Destroy(bullet, 5f);
       
        
    }
    
  
}


