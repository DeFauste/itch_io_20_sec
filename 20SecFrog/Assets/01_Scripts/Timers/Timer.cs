using System;
using System.Collections;
using MainMenu;
using TMPro;
using UnityEngine;

namespace _01_Scripts.Timer
{
    /// <summary>
    /// Клас отсчёта времени на экране
    /// </summary>
    public sealed class Timer : MonoBehaviour
    {
        // Время с которого будет идти отсчёт времени
        [SerializeField] private int timeMax = 20;

        // Скорость с которой будет изменяться значение таймера
        [SerializeField] private float timerSpeedChange = 0.1f;

        // Ссылка на UI для вывода значения таймера
        [SerializeField] private TextMeshProUGUI timerText;

        // Округление значение таймера
        [SerializeField] private int roundTimer = 3;

        // Время для таймера
        public double TimerValue { get; private set; }
        private bool isPaused = true;

        public void PauseTimer() => isPaused = true;
        public void ResumeTimer() => isPaused = false;
        public void ResetTimer() => TimerValue = timeMax;
        
        // Включеа ли уже коротина
        private bool startCorutine = false;

        // Сохраняем ссылку на корутину
        private Coroutine coroutine;

        private void Start()
        {
            TimerValue = timeMax;
        }

        private void Update()
        {
            if (timerText is not null)
            {
                timerText.text = $"{TimerValue}";
            }
        }

        public void SartTimer()
        {
            if (!startCorutine)
            {
                startCorutine = true;
                Debug.Log("Starting Timer");
                coroutine = StartCoroutine(StartTimer(timeMax));
                ResumeTimer();
            }
        }

        private IEnumerator StartTimer(int seconds)
        {
            TimerValue = seconds;
            float time = seconds;
            while (time > 0 && startCorutine)
            {
                yield return isPaused ? null : new WaitForSeconds(timerSpeedChange);
                if (isPaused == false)
                {
                    time -= timerSpeedChange;
                    TimerValue = Math.Round(time, roundTimer);
                }
            }

            startCorutine = false;
        }
        
        public void StopTimer()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                startCorutine = false;
            }
        }
    }
}