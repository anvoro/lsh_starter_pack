using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Lecture2
{
    public class ScoreCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private float _duration = 2;
        private int displayedScore = 0;
        private Tween counterTween;

        private int _score = 100;
        
        public void UpdateScoreAnimated(int targetScore, float duration = 0.5f)
        {
            // Останавливаем предыдущую анимацию счета, если она идет
            counterTween?.Kill();

            // Магия DOTween: анимируем обычную переменную displayedScore от текущего значения до targetScore
            counterTween = DOTween.To(() => displayedScore, x => displayedScore = x, targetScore, duration)
                .SetEase(Ease.OutQuad)
                .OnUpdate(() =>
                {
                    // Этот блок вызывается каждый кадр изменения переменной
                    _scoreText.text = displayedScore.ToString();
                });
            
            // Добавим сочности: заставим текст слегка пульсировать во время подсчета
            _scoreText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), duration, 5, 0.5f);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UpdateScoreAnimated(_score, _duration);
                _score += 100;
            }
        }
    }
}