using System;
using UnityEngine;

namespace Lecture1.Observer
{
    public class PlayerHealth : MonoBehaviour
    {
        // Статическое событие. Передает текущее здоровье.
        // Подобный подход (со статическим событием), подходит только если объект вызывающий событие всегда гарантированно один (как игрок в одиночной игре)
        // Если же таких объектом много (например врагов), то статическое событие не подойдёт, нужно делать его обычным полем, уникальным для каждого экземпляра класса
        public static event Action<int> OnHealthChanged;

        [SerializeField] private int _maxHealth = 100;
        
        private int _currentHealth;

        private void Start() => _currentHealth = _maxHealth;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
        
            // ?.Invoke проверяет, подписан ли хоть кто-то на это событие
            // Здесь мы передаём информацию (контекст события) вместе с его вызовом, эта информация может быть сложнее одного поля
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }
}