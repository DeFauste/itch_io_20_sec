using UnityEngine;

namespace _01_Scripts.Frogs
{
    public class UpdateFader : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Color startColor;
        [Header("Настройки затухания")] public float fadeSpeed = 0.5f;
        private bool isFading = false;
        private float targetAlpha = 0f;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startColor = spriteRenderer.color;
        }

        void Update()
        {
            if (isFading)
            {
                FadeUpdate();
            }
        }

        void FadeUpdate()
        {
            Color currentColor = spriteRenderer.color;
            float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);

            currentColor.a = newAlpha;
            spriteRenderer.color = currentColor;

            if (Mathf.Approximately(newAlpha, targetAlpha))
            {
                isFading = false;
                OnFadeComplete();
            }
        }

        public void StartFade(float targetAlpha = 0f)
        {
            this.targetAlpha = targetAlpha;
            isFading = true;
        }

        void OnFadeComplete()
        {
            spriteRenderer.color = startColor;
            isFading = false;
            gameObject.SetActive(false);
        }
    }
}