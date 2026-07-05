using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class CameraFovPulse : MonoBehaviour
    {
        private Camera mainCamera;
        private float defaultFov;

        [Header("Pulse Settings")]
        [SerializeField] private float targetFov = 55f;       // FOV при максимальном приближении (дефолт обычно 60)
        [SerializeField] private float kickDuration = 0.15f;   // Время быстрого приближения (удара сердца)
        [SerializeField] private float returnDuration = 0.4f;  // Время плавного возврата камеры в норму
        [SerializeField] private float pulseInterval = 1.0f;   // Интервал между ударами сердца в секундах

        private float timer;

        private void Awake()
        {
            mainCamera = GetComponent<Camera>();
            defaultFov = mainCamera.fieldOfView;
        
            // Инициализируем таймер, чтобы первая пульсация произошла через pulseInterval секунд
            timer = pulseInterval; 
        }

        private void Update()
        {
            // Простейший таймер в Update
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                TriggerPulse();
                timer = pulseInterval; // Сбрасываем таймер обратно
            }
        }

        private void TriggerPulse()
        {
            // Безопасность: прерываем предыдущую анимацию FOV, если она еще не завершилась
            mainCamera.DOComplete();

            // Ритм сердца состоит из двух фаз:
            // 1. Быстрое сжатие (уменьшение FOV приближает камеру)
            mainCamera.DOFieldOfView(targetFov, kickDuration)
                .SetEase(Ease.OutQuad)
                // 2. Плавный возврат в исходное состояние по завершении первой фазы
                .OnComplete(() =>
                {
                    mainCamera.DOFieldOfView(defaultFov, returnDuration).SetEase(Ease.InOutQuad);
                });
        }

        private void OnDestroy()
        {
            // Обязательно очищаем твины камеры при уничтожении скрипта
            mainCamera.DOKill();
        }
    }
}
