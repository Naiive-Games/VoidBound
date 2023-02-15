using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymove : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;

    void FixedUpdate()
    {
       // Направление к игроку
        Vector3 direction = player.position - transform.position;

    // Повернуть объект в направлении игрока
        transform.LookAt(player);

    // Двигаться в направлении игрока
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World); 
    }
}
