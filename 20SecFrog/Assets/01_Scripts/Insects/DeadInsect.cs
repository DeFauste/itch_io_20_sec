using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class DeadInsect : MonoBehaviour
{
    [SerializeField] private float _timeBeforeFade = 2f;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        
        int rotation = Random.Range(0, 18) * 20;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        _scoreText.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _fadeCoroutine = StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(_timeBeforeFade);
        var estimatedTime = 0f;
        var targetAlpha = 0f;
        _scoreText.gameObject.SetActive(false);
        while (estimatedTime < _fadeTime)
        {
            var percent = estimatedTime / _fadeTime;
            Color currentColor = _spriteRenderer.color;
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, percent);
            currentColor.a = newAlpha;
            _spriteRenderer.color = currentColor;
            estimatedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}