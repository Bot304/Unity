using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public Vector3 offset = new Vector3(0, 3, -10); // Смещение камеры
    public float smoothTime = 0.3f; // Время сглаживания движения камеры

    private Vector3 currentVelocity; // Переменная для расчёта скорости SmoothDamp

    void FixedUpdate()
    {
        if (player != null)
        {
            // Целевая позиция камеры с учётом смещения
            Vector3 targetPosition = player.position + offset;

            // Плавное движение камеры к целевой позиции
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
    }

}
