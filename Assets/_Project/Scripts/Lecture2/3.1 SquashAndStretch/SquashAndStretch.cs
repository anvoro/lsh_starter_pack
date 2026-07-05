using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class SquashAndStretch : MonoBehaviour
    {
            [Header("Physics")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Juice Visuals (Child Object)")]
    [SerializeField] private Transform visualTransform; // Ссылка на дочерний объект графики

    private bool isGrounded;
    private bool wasGroundedLastFrame;

    private void Update()
    {
        // 1. Проверяем, стоит ли персонаж на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. Эффект приземления (Land):
        // Если в прошлом кадре мы летели (wasGroundedLastFrame == false), 
        // а в этом кадре коснулись земли (isGrounded == true) — запускаем эффект плющения
        if (isGrounded && !wasGroundedLastFrame)
        {
            PlayLandEffect();
        }

        // 3. Прыжок на Пробел
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Запоминаем состояние заземления для следующего кадра
        wasGroundedLastFrame = isGrounded;
    }

    private void Jump()
    {
        // Физический толчок вверх
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // Визуальный эффект прыжка (вытягивание вверх)
        visualTransform.DOComplete(); // Сбрасываем предыдущие твины
        visualTransform.localScale = Vector3.one;

        // Быстро сжимаем по бокам (X = 0.7) и вытягиваем вверх (Y = 1.3) за 0.1 сек,
        // а затем плавно возвращаем к исходному размеру (Vector3.one) за 0.15 сек
        visualTransform.DOScale(new Vector3(0.7f, 1.3f, 1f), 0.1f)
            .OnComplete(() => visualTransform.DOScale(Vector3.one, 0.15f));
    }

    private void PlayLandEffect()
    {
        visualTransform.DOComplete(); // Сбрасываем предыдущие твины
        visualTransform.localScale = Vector3.one;

        // Плющим по вертикали (Y = 0.6) и расширяем по бокам (X = 1.4) за 0.1 сек,
        // после чего плавно возвращаем в норму за 0.2 сек
        visualTransform.DOScale(new Vector3(1.4f, 0.6f, 1f), 0.1f)
            .OnComplete(() => visualTransform.DOScale(Vector3.one, 0.2f));
    }

    private void OnDrawGizmos()
    {
        // Рисуем в редакторе красную сферу проверки земли для удобства настройки
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnDestroy()
    {
        // Убиваем твины при уничтожении объекта, чтобы избежать ошибок в памяти
        visualTransform.DOKill();
    }
    }
}