using _01_Scripts.Common;
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
        public bool Paused => menu.Paused;
        private void Update()
        {
            if (timerGame?.TimerValue <= 0)
            {
                menu.PauseGame();
            }
        }
    }
}