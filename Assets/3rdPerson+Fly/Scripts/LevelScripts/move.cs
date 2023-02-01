using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveObject : MonoBehaviour
{
    public float speed = 10.0f; // Определяет скорость движения объекта
    public float rotationSpeed = 10.0f; // Определяет скорость вращения объекта

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Получаем значение горизонтальной оси
        float vertical = Input.GetAxis("Vertical"); // Получаем значение вертикальной оси

        // Обновляем позицию объекта в соответствии с нажатием на клавиатуру
        transform.position = transform.position + transform.forward * vertical * speed * Time.deltaTime;

        // Обновляем вращение объекта в соответствии с нажатием на клавиатуру
        transform.rotation = transform.rotation * Quaternion.AngleAxis(horizontal * rotationSpeed * Time.deltaTime, Vector3.up);
    }

}
