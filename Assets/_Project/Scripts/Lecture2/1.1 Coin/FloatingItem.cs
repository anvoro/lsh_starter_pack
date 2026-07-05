using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class FloatingItem : MonoBehaviour
    {
        [SerializeField] private float floatDistance = 0.5f; // Высота покачивания
        [SerializeField] private float duration = 1f;        // Время движения в одну сторону

        private void Start()
        {
            // Плавно перемещаем по оси Y относительно стартовой позиции
            transform.DOMoveY(transform.position.y + floatDistance, duration)
                .SetEase(Ease.InOutQuad)   // Плавное ускорение в начале и замедление в конце
                .SetLoops(-1, LoopType.Yoyo); // -1 означает бесконечный цикл, Yoyo — движение туда-обратно
        }

        private void OnDestroy()
        {
            // Хорошая привычка: всегда уничтожать твины при уничтожении объекта
            transform.DOKill();
        }
    }
}
