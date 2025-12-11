using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class LogoLoader : MonoBehaviour
    {
        #region CHANGE ALPHA LOADER LOGO
        [SerializeField] private Image _image;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private Canvas _canvasLogo;
        [SerializeField] private float TimeShowLogo = 1;
        private Coroutine _fadeRoutine;

        private void Awake()
        {
            _image.enabled = true;
        }

        private void Start()
        {
            StartCoroutine(StartTimer(3, () => StartFade(0)));
        }

        private void StartFade(float targetAlpha)
        {
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);

            _fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
        }
        
        private IEnumerator StartTimer(int seconds, Action action)
        {
            float time = seconds;
            while (time > 0)
            {
                yield return new WaitForSeconds(1);

                time -= 1;
            }
            action?.Invoke();
        }
        private IEnumerator FadeTo(float targetAlpha)
        {
            float startAlpha = _image.color.a;
            float elapsedTime = 0f;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeDuration);
                SetAlpha(newAlpha);
                yield return null;
            }

            SetAlpha(targetAlpha);
            if (_canvasLogo != null)
            {
                _canvasLogo.enabled = false;
            }
        }

        private void SetAlpha(float alpha)
        {
            Color color = _image.color;
            color.a = alpha;
            _image.color = color;
        }
        #endregion CHANGE ALPHA LOADER LOGO
    }
}