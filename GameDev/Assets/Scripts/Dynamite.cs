using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public GameObject explosionEffect; // Префаб взрыва
    public float explosionDelay = 1f; // Время до взрыва
    public float explosionRadius = 5f; // Радиус взрыва
    public float explosionForce = 10f; // Сила откидывания
    public float upwardsModifier = 1f; // Направление силы вверх

    private bool hasExploded = false;
    private bool hasLanded = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;

            // Останавливаем движение, но не ставим isKinematic в true
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Запускаем таймер взрыва через некоторое время
            Invoke("Explode", explosionDelay);
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Создаем взрывный эффект
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Находим все объекты в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            // Применяем силу взрыва ко всем объектам
            Rigidbody targetRb = collider.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                // Получаем направление от взрыва
                Vector3 explosionDirection = collider.transform.position - transform.position;
                explosionDirection.Normalize();

                // Откидываем объекты
                targetRb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
                targetRb.AddForce(Vector3.up * upwardsModifier, ForceMode.Impulse);
            }

            // Откидываем игрока
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyExplosionForce(transform.position, explosionRadius, explosionForce);
            }

            // Откидываем зомби
            ZombieController zombie = collider.GetComponentInParent<ZombieController>();
            if (zombie != null)
            {
                // Откидываем зомби
                zombie.ApplyExplosionForce(transform.position, explosionForce);
            }
        }

        // Уничтожаем динамит
        Destroy(gameObject);
    }
}
