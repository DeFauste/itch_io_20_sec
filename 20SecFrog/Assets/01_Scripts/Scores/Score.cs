using TMPro;
using UnityEngine;

namespace _01_Scripts.Score
{
    public sealed class Score : MonoBehaviour
    {
        // Ссылка на UI для вывода очков счёта
        [SerializeField] private TextMeshProUGUI scoreText;
        private double scoreValue = 0d;

        public void Add()
        {
            AddScore(100);
        }
        
        // Добавить очки
        public void AddScore(double value)
        {
            scoreValue += value;
            scoreText.text = $"{scoreValue}";
        }
        
        // Вычесть очки
        public void DiffScore(double value)
        {
            scoreValue -= value;
            scoreText.text = $"{scoreValue}";
        }
    }
}