using UnityEngine;
public class ZombieController : MonoBehaviour
{
    public Animator animator; // Ссылка на аниматор
    public Rigidbody rb; // Rigidbody зомби

    private bool isDead = false;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    // Метод для проигрывания анимации смерти
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        // Включаем анимацию смерти (например, "Die")
        animator.SetTrigger("Die");
    }
     
    // Метод для откидывания зомби
    public void ApplyExplosionForce(Vector3 explosionCenter, float explosionForce)
    {
        if (isDead) return; // Если зомби мертв, не применяем силу

        // Важно: вызовем смерть сразу перед применением силы
        Die();

        // Откидываем зомби в сторону взрыва
        Vector3 explosionDirection = rb.position - explosionCenter;
        explosionDirection.Normalize();

        // Применяем силу от взрыва
        rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 2f, ForceMode.Impulse); // Это можно дополнительно для эффекта откидывания вверх
    }
}
