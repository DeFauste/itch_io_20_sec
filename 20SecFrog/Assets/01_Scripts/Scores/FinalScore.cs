using TMPro;
using UnityEngine;

namespace _01_Scripts.Score
{
    public class FinalScore : MonoBehaviour
    {
        [SerializeField] Score score;
        [SerializeField] private TextMeshProUGUI scoreText;

        public void UpdateScore()
        {
            scoreText.text = $"SCORE: {score.ScoreValue}";
            score.ResetScore();
        }
        
    }
}