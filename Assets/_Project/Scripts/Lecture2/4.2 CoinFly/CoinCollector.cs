using DG.Tweening;
using UnityEngine;

namespace Lecture2
{
    public class CoinCollector : MonoBehaviour
    {
        [SerializeField] private RectTransform coinUIRectTarget; // Иконка монетки в UI
        [SerializeField] private GameObject coinPrefab;          // Префаб монетки в мире
        [SerializeField] private Camera mainCamera;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnAndCollect(Vector3.zero);
            }
        }
        
        public void SpawnAndCollect(Vector3 spawnWorldPos)
        {
            // 1. Спавним физическую монетку в мире
            GameObject coin = Instantiate(coinPrefab, spawnWorldPos, Quaternion.identity);

            // 2. Вычисляем мировую позицию цели (иконки UI) на плоскости игры
            // Переводим позицию UI (Screen Space) в мировые координаты (World Space)
            Vector3 targetWorldPos = mainCamera.ScreenToWorldPoint(coinUIRectTarget.position);
            targetWorldPos.z = 0; // Обнуляем Z для 2D

            // 3. Создаем последовательность (Sequence)
            Sequence coinSequence = DOTween.Sequence();

            // Шаг А: Монетка эффектно "выпрыгивает" вверх-вбок из точки спавна
            Vector3 jumpPos = coin.transform.position + new Vector3(Random.Range(-1f, 1f), 1.5f, 0f);
            coinSequence.Append(coin.transform.DOMove(jumpPos, 0.3f).SetEase(Ease.OutQuad));

            // Шаг Б: Небольшая пауза в воздухе (зависание на 0.1 сек)
            coinSequence.AppendInterval(0.1f);
                
            // Шаг В: Монетка летит в UI-цель и одновременно уменьшается
            coinSequence.Append(coin.transform.DOMove(targetWorldPos, 0.5f).SetEase(Ease.InBack));
            coinSequence.Join(coin.transform.DOScale(new Vector3(0.5f ,0.5f, 0.5f), 0.5f)); // Выполняется ОДНОВРЕМЕННО с полетом

            // Шаг Г: Событие по завершении полета
            coinSequence.OnComplete(() =>
            {
                Destroy(coin); // Удаляем монетку

                // Трясем иконку UI, показывая, что монетка "попала в цель"
                coinUIRectTarget.DOComplete();
                coinUIRectTarget.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.15f, 10, 0.5f);
            });
        }
    }
}
