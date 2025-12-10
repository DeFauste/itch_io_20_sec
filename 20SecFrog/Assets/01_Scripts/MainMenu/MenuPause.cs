using System.Collections;
using System.Collections.Generic;
using _01_Scripts.Frogs;
using _01_Scripts.Timer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class MenuPause : MonoBehaviour
    {
        // Меню
        [SerializeField] RectTransform logo;
        [SerializeField] RectTransform playQuit;
        [SerializeField] RectTransform buttomQuit;
        [SerializeField] private float duration = 1f;
        [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField] private Button playGameButton;
        [SerializeField] private Button quitGameButton;
        bool isPaused = true;
        private bool blockerMenu = false; // блокируем объюз меню

        public bool Paused => isPaused;
        // Меню
        
        // Таймер и очки
        [SerializeField] private Timer timerGame;
        [SerializeField] GameObject timerObject;
        [SerializeField] GameObject scoreObject;
        //

        [SerializeField] private InsectCapture insectCapture;
        [SerializeField] private List<Spawner> spawners = new List<Spawner>();

        [SerializeField] private MusicPlayer player;

        private void OnEnable()
        {
            playGameButton.onClick.AddListener(() => StartGame());
            quitGameButton.onClick.AddListener(() => ExitGame());
        }

        private void Awake()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }


        public void ExitGame()
        {
            Application.Quit();
        }

        public void PauseGame()
        {
            if (!blockerMenu && !isPaused)
            {
                blockerMenu = true;
                isPaused = true;
                insectCapture.DeactiveCursor();
                MoveToPoint(logo, -1000, 1000);
                MoveToPoint(buttomQuit, 1000, 1000);
                MoveToPoint(playQuit, 1000, 1000);
                timerObject?.SetActive(false);
                scoreObject?.SetActive(false);
                timerGame.PauseTimer();
            }
        }

        private void StartGame()
        {
            if (!blockerMenu && isPaused)
            {
                blockerMenu = true;
                isPaused = false;
                Time.timeScale = 1;
                insectCapture.ActiveCursor();
                if (timerGame.TimerValue >= 19)
                {
                    StartSpawners();
                }

                MoveToPoint(logo, 1000, 1000);
                MoveToPoint(buttomQuit, -1000, 1000);
                MoveToPoint(playQuit, -1000, 1000);
                timerGame?.SartTimer();
                timerObject?.SetActive(true);
                scoreObject?.SetActive(true);
                timerGame?.ResumeTimer();

                player.TriggerEvent();
            }
        }

        private void StartSpawners()
        {
            foreach (var spawner in spawners)
            {
                spawner.StartSpawning();
            }
        }

        public void MoveToPoint(RectTransform uiElement, float distance = 1000f, float speed = 200)
        {
            StartCoroutine(MoveUpSmooth(uiElement, distance, speed));
        }

        public IEnumerator MoveUpSmooth(RectTransform img, float distance = 1000f, float speed = 200f)
        {
            Vector2 target = img.anchoredPosition + new Vector2(0, distance);

            while (Vector2.Distance(img.anchoredPosition, target) > 0.1f)
            {
                img.anchoredPosition = Vector2.MoveTowards(
                    img.anchoredPosition,
                    target,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            blockerMenu = false;
            img.anchoredPosition = target;
        }
    }
}