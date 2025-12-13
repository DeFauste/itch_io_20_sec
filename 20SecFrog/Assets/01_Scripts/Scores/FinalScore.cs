using System;
using TMPro;
using UnityEngine;

namespace _01_Scripts.Score
{
    public class FinalScore : MonoBehaviour
    {
        [SerializeField] Score score;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject scoreObject;

        public void UpdateScore()
        {
            scoreText.text = $"SCORE: {score.ScoreValue}";
            score.ResetScore();
            scoreObject.SetActive(true);
        }
        
    }
}