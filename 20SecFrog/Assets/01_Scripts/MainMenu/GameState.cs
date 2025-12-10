using System;
using _01_Scripts.Common;
using _01_Scripts.Score;
using _01_Scripts.Timer;
using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Контролируем состояние игры
    /// </summary>
    public sealed class GameState : SingletonMonoBehaviour<GameState>
    {
        [SerializeField] public Timer timerGame;
        [SerializeField] public MenuPause menu;
        [SerializeField] private float speedInsectPercent = 1;
        [SerializeField] private FinalScore  finalScore;
        public bool Paused => menu.Paused;
        
        public float GetGlobalSpeed() => speedInsectPercent;
        
        public void SetSpeedInsectPercent(float percent) => speedInsectPercent = percent;
        public void ResetSpeedInsectPercent() => speedInsectPercent = 1;
        private void Update()
        {
            if (timerGame?.TimerValue < 0)
            {
                timerGame?.ResumeTimer();
                timerGame?.ResetTimer();
                finalScore.UpdateScore();
                menu.PauseGame();
                timerGame.StopTimer();
                DestroyAllInsect();
            }
        }

        private void DestroyAllInsect()
        {
            InsectBase[] insects = FindObjectsOfType<InsectBase>(true);
            foreach (var insect in insects)
            {
                Destroy(insect.gameObject);
            }
        }
    }
}