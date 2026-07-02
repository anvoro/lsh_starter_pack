using UnityEngine;

namespace Lecture1.Singleton
{
    // Наследуемся от SingletonBase и передаем сам класс в качестве параметра, здесь GameManager == T из SingletonBase
    public class GameManager : SingletonBase<GameManager>
    {
        public int Score { get; private set; }

        public void AddScore(int amount)
        {
            Score += amount;
            Debug.Log($"Очки: {Score}");
        }
    }

    // Использование в коде:
    // GameManager.Instance.AddScore(10);
    // Здесь GameManager - это тип нашего класса, Instance - Статическое свойство, в котором храниться инстанс (единственный в памяти) типа GameManager
    // AddScore, это НЕ статический метод (метод экземпляра)
}