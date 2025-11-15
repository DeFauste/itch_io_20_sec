using System;
using System.Collections;
using TMPro;
using UnityEditor;
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
        // Скорость с которой будет изменятся значение таймера
        [SerializeField] private float timerSpeedChange = 0.1f;
        // Ссылка на UI для вывода значения таймера
        [SerializeField] private TextMeshProUGUI timerText;
        // Округление значение таймера
        [SerializeField] private int roundTimer = 3;
        
        // Время для таймера
        public double TimerValue { get; private set; }

        // Включеа ли уже коротина
        private bool startCorutine = false;

        // Сохраняем ссылку на корутину
        private Coroutine coroutine;

        private void Start()
        {
            TimerValue =  timeMax;
        }

        private void Update()
        {
            if (timerText is not null)
            {
                timerText.text = $"{TimerValue} s";
            }
        }

        public void SartTimer()
        {
            if (!startCorutine)
            {
                startCorutine = true;
                coroutine = StartCoroutine(StartTimer(timeMax));
            }
        }

        private IEnumerator StartTimer(int seconds)
        {
            TimerValue = seconds;
            float time = seconds;

            while (time > 0 && startCorutine)
            {
                yield return new WaitForSeconds(timerSpeedChange);
                time -= timerSpeedChange ;
                TimerValue = Math.Round(time, 3);
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