using _01_Scripts.Timer;
using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Контролируем состояние игры
    /// </summary>
    public sealed class GameState : MonoBehaviour
    {
        [SerializeField] public Timer timerGame;
        [SerializeField] public MenuPause menu;

        private void Update()
        {
            if (timerGame?.TimerValue <= 0)
            {
                menu.PauseGame();
            }
        }
        
    }
}