using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class UIPanelPopup : MonoBehaviour
    {
        [SerializeField] private float duration = 0.4f;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [ContextMenu("Show")]
        public void Show()
        {
            gameObject.SetActive(true);
        
            // Сбрасываем параметры перед анимацией
            transform.localScale = Vector3.zero;
            if (canvasGroup != null) canvasGroup.alpha = 0f;

            // Анимируем размер с эффектом отскока (Ease.OutBack)
            transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        
            // Параллельно плавно проявляем прозрачность
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(1f, duration);
            }
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            // При скрытии плавно уменьшаем и выключаем объект по завершении
            transform.DOScale(Vector3.zero, duration - 0.1f).SetEase(Ease.InBack);
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0f, duration - 0.1f).OnComplete(() => gameObject.SetActive(false));
            }
        }
    }
}
