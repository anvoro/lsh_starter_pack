using UnityEngine;
using UnityEngine.UI;

namespace Lecture1.Observer
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;

        private void OnEnable()
        {
            // Подписываемся на событие
            // На события можно подписываться несколько раз из различных мест, например можно опдписаться из звуковой системы, чтобы проигрвыать звук получения удара итп
            // Главное, не забывать отписываться ровно столько раз, сколько мы подписались
            PlayerHealth.OnHealthChanged += UpdateUI;
        }

        private void OnDisable()
        {
            // ОБЯЗАТЕЛЬНО отписываемся при уничтожении объекта!
            // Иначе получим утечку памяти и NullReferenceException
            PlayerHealth.OnHealthChanged -= UpdateUI;
        }

        // Сигнатура метода (int currentHealth) должна совпадать с сигнатурой события, на которое он подписывается (Action<int>)
        private void UpdateUI(int currentHealth)
        {
            _healthSlider.value = currentHealth;
        }
    }
}