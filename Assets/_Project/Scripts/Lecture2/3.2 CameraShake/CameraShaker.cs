using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _strength = 0.4f;
        
        private Vector3 originalPosition;

        private void Start()
        {
            originalPosition = transform.position;
        }

        public void Shake(float duration = 0.25f, float strength = 0.4f)
        {
            // Сбрасываем камеру в дефолт, чтобы тряска не накапливала смещение
            transform.DOComplete();
            transform.position = originalPosition;

            // Трясем позицию. Параметры: длительность, сила, вибрация (частота), случайность направления
            transform.DOShakePosition(duration, strength, 12, 90, false, true);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shake(_duration, _strength);
            }
        }
    }
}
