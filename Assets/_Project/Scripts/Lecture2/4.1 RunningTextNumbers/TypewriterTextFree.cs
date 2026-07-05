using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Lecture2
{
    public class TypewriterTextFree : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent;
        [SerializeField] private float typingSpeed = 30f; // Скорость: сколько букв в секунду

        private int visibleCharactersCount = 0;
        private Tween typingTween;

        [ContextMenu("Test Typewriter")]
        public void StartTypingTest()
        {
            ShowText("Привет, путник! Я в <color=red>V AHUE</color>!");
        }

        public void ShowText(string message)
        {
            // Прерываем предыдущую анимацию
            typingTween?.Kill();

            // 1. Сразу записываем полный текст в компонент
            textComponent.text = message;
        
            // 2. Скрываем все символы (делаем видимыми 0 символов)
            textComponent.maxVisibleCharacters = 0;
            visibleCharactersCount = 0;

            // Вычисляем длительность на основе длины текста
            float duration = message.Length / typingSpeed;

            // 3. Магия DOTween: плавно меняем переменную visibleCharactersCount от 0 до длины текста
            typingTween = DOTween.To(
                    () => visibleCharactersCount,          // Геттер переменной
                    x => visibleCharactersCount = x,       // Сеттер переменной
                    message.Length,                        // Целевое значение (длина строки)
                    duration                               // Длительность
                )
                .SetEase(Ease.Linear) // Равномерная печать
                .OnUpdate(() =>
                {
                    // Каждый кадр обновляем количество видимых символов в TextMeshPro
                    textComponent.maxVisibleCharacters = visibleCharactersCount;
                })
                .OnComplete(() =>
                {
                    // На всякий случай в конце открываем все символы
                    textComponent.maxVisibleCharacters = message.Length;
                });
        }

        // Пропуск анимации (Game Feel)
        public void SkipTyping()
        {
            if (typingTween != null && typingTween.IsActive() && typingTween.IsPlaying())
            {
                typingTween.Complete(); // Мгновенно завершает твин
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                SkipTyping();
            }
        }

        private void OnDestroy()
        {
            typingTween?.Kill();
        }
    }
}
