using TMPro;
using UnityEngine;

namespace _01_Scripts.Score
{
    public sealed class Score : MonoBehaviour
    {
        // Ссылка на UI для вывода очков счёта
        [SerializeField] private TextMeshProUGUI scoreText;
        private double scoreValue = 0d;
        public double ScoreValue => scoreValue;
        public void ResetScore()
        {
            scoreValue = 0;
            scoreText.text = $"SCORE: {scoreValue}";
        }

        public void Add()
        {
            AddScore(100);
        }
        
        // Добавить очки
        public void AddScore(double value)
        {
            scoreValue += value;
            scoreText.text = $"SCORE: {scoreValue}";
        }
        
        // Вычесть очки
        public void DiffScore(double value)
        {
            scoreValue -= value;
            scoreText.text = $"SCORE: {scoreValue}";
        }
    }
}