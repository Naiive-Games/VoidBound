using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TopDownCamera : MonoBehaviour
{
    public Transform target; // Определяет цель, к которой присоединена камера
    public float height = 10.0f; // Определяет высоту, с которой смотрит камера

    void LateUpdate()
    {
        // Устанавливаем позицию камеры выше цели на заданную высоту
        transform.position = target.position + new Vector3(0, height, 0);
        // Направляем камеру на цель
        transform.LookAt(target);
    }
}
