using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class DamageFlash : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        [SerializeField] float _duration = 0.5f;
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }

        [ContextMenu("Flash")]
        public void Flash()
        {
            // Прерываем текущее мигание, возвращая цвет в исходный
            spriteRenderer.DOComplete();
            spriteRenderer.color = originalColor;

            // Мгновенно делаем спрайт красным и за _duration сек плавно возвращаем исходный цвет
            spriteRenderer.color = Color.red;
            spriteRenderer.DOColor(originalColor, _duration).SetEase(Ease.OutQuad);
        }
    }
}