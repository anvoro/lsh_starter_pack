using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private Vector3 movementOffset = new Vector3(4f, 0f, 0f); // Смещение относительно старта
        [SerializeField] private float duration = 2f;                             // Время хода в одну сторону

        [Header("DOTween Settings")]
        [SerializeField] private Ease easeType = Ease.InOutQuad;                  // Тип изинга (сглаживания)

        private Vector3 startPosition;
        private Tween moveTween;

        private void Start()
        {
            startPosition = transform.position;

            // Вычисляем конечную точку движения
            Vector3 targetPosition = startPosition + movementOffset;

            // Запускаем движение к цели
            moveTween = transform.DOMove(targetPosition, duration)
                .SetEase(easeType)             // Применяем выбранный изинг
                .SetLoops(-1, LoopType.Yoyo);  // Бесконечно (-1) туда-обратно (Yoyo)
        }

        private void OnDestroy()
        {
            // Убиваем твин при уничтожении платформы, чтобы избежать утечек памяти
            moveTween?.Kill();
        }

        // ВАЖНО ДЛЯ ГЕЙМДИЗАЙНА: Визуализируем путь платформы в редакторе Unity!
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
        
            // Если игра запущена — берем сохраненный старт, если нет — текущую позицию в редакторе
            Vector3 start = Application.isPlaying ? startPosition : transform.position;
            Vector3 end = start + movementOffset;

            // Рисуем линию пути и коробку в конечной точке
            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireCube(end, transform.localScale);
        }
    }
}
