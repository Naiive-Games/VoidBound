using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    
    public float shootingRange = 10f; // дистанция стрельбы
    public float shootingInterval = 1f; // интервал между выстрелами
    public float bulletSpeed = 10f; // скорость снаряда
    public GameObject bulletPrefab; // префаб снаряда
    public Transform shootingPoint; // точка, откуда происходит стрельба
  
    public GameObject player; // игрок
    private float lastShotTime; // время последнего выстрела

 

    private void Update()
    {
        // проверяем, есть ли игрок в пределах дистанции стрельбы
        if (Vector3.Distance(player.transform.position, transform.position) < shootingRange)
        {

            // проверяем, прошло ли достаточно времени для следующего выстрела
            if (Time.time > lastShotTime + shootingInterval)
            {
                // создаем снаряд
                GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

                // запускаем снаряд в сторону игрока
                bullet.GetComponent<Rigidbody>().velocity = (player.transform.position - shootingPoint.position).normalized * bulletSpeed;

                lastShotTime = Time.time; // обновляем время последнего выстрела
            }
        }
    }

}
