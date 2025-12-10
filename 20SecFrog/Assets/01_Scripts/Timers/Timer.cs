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
        [SerializeField] private float timeMax = 20.9f;

        // Ссылка на UI для вывода значения таймера
        [SerializeField] private TextMeshProUGUI timerText;

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

        private IEnumerator StartTimer(float seconds)
        {
            TimerValue = seconds;
            float time = seconds;
            while (time > -1 && startCorutine)
            {
                if (isPaused)
                {
                    yield return null;
                }

                TimerValue = Math.Floor(time);
                time -= Time.deltaTime;
                yield return null;
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