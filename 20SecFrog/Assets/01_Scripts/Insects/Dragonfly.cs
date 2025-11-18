using System.Collections;
using UnityEngine;

public class Dragonfly : InsectBase
{
    [SerializeField] private float _moveDuration;
    [SerializeField] private int _movesCount;
    [SerializeField] private float _suicideTime = 2f;
    [SerializeField] private AnimationCurve _curve;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _currentScale;
    private Vector3 _newScale;
    private float _scaleMultiplier;
    private float _elapsedTime;
    
    void Start()
    {
        _moveDuration /= _speed;
        _startPosition = transform.position;
        _currentScale = transform.localScale;
        _scaleMultiplier = Random.Range(0.8f, 1.2f);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
      
        while (_movesCount > 0)
        {
            _newScale = Vector3.one * _scaleMultiplier;
            _endPosition = new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), 0f);;
             while (_elapsedTime < _moveDuration)
             {
               _elapsedTime += Time.deltaTime;
               float percentageComplete = _elapsedTime / _moveDuration;
               transform.position = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(percentageComplete));
               transform.localScale = Vector3.Lerp(_currentScale, _newScale, _curve.Evaluate(percentageComplete));
               yield return null;
             }

             _elapsedTime = 0f;
             _startPosition = transform.position;
             _endPosition = new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), 0f);
             _currentScale = transform.localScale;
             _scaleMultiplier = Random.Range(0.8f, 1.2f);
             _newScale = _currentScale * Mathf.Pow(1.2f, _scaleMultiplier);
             
             _movesCount--;
        }
        
        float suicideAngle = Random.Range(-5f, 185f);
        float suicideRadius = 12f;
        float x = Mathf.Cos(suicideAngle) * suicideRadius;
        float y = Mathf.Sin(suicideAngle) * suicideRadius;
        _endPosition = new Vector3(x, y, 0f);
        while (_elapsedTime < _suicideTime)
        {
            _elapsedTime += Time.deltaTime;
            float percentageComplete = _elapsedTime / _suicideTime;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(percentageComplete));
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
