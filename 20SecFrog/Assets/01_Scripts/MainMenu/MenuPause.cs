using System;
using _01_Scripts.Frogs;
using _01_Scripts.Timer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class MenuPause : MonoBehaviour
    {
        [SerializeField] private Button restartGameButton;
        [SerializeField] private Button exitGameButton;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private InsectCapture insectCapture;
        [SerializeField] private Timer timerForStart;
        [SerializeField] private GameObject timerObject;

        private void OnEnable()
        {
            restartGameButton.onClick.AddListener(() => StartGame());
            exitGameButton.onClick.AddListener(() => ExitGame());
        }

        private void Awake()
        {
            PauseGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                PauseGame();
            }

            if (timerForStart.TimerValue <= 0)
            {
                Time.timeScale = 1;
                timerForStart.StopTimer();
            }
        }


        public void ExitGame()
        {
            SceneManager.LoadScene("00_MainMenu");
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            insectCapture.DeactiveCursor();
        }

        private void StartGame()
        {
            Time.timeScale = 1;
            // прикрутить старт спвна и всего движения 
            pauseMenu.SetActive(false);
            insectCapture.ActiveCursor();
            timerObject.SetActive(true);
            timerForStart.SartTimer();
        }
    }
}