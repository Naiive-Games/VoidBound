using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public Transform target; // Определяет цель, которую должна следовать камера
    public float smoothSpeed = 0.125f; // Определяет скорость плавного движения камеры
    public Vector3 offset; // Определяет отступ камеры от цели

    void LateUpdate()
    {
        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = target.position + offset;
        // Интерполируем позицию камеры между текущей и желаемой с заданной скоростью
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Обновляем позицию камеры
        transform.position = smoothedPosition;
    }
}

