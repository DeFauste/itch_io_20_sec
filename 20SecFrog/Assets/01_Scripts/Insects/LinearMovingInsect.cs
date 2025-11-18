using System.Collections;
using UnityEngine;

public class LinearMovingInsect : InsectBase
{
    [SerializeField] protected float _moveDuration;
    [SerializeField] protected int _movesCount;
    [SerializeField] protected float _minScale;
    [SerializeField] protected float _maxScale;
    [SerializeField] protected float _firstMoveDuration;

    [SerializeField] protected float _suicideTime = 2f;
    [SerializeField] protected AnimationCurve _curve;

    [Header("Ограничения под size камеры")] [SerializeField]
    protected float _maxX = 9f;

    [SerializeField] protected float _minX = -9f;
    [SerializeField] protected float _maxY = 4f;
    [SerializeField] protected float _minY = -3f;
    [SerializeField] protected float _suicideRadius = 12f;

    protected Vector3 _startPosition;
    protected Vector3 _endPosition;
    protected Vector3 _currentScale;
    protected Vector3 _newScale;
    protected float _scaleMultiplier;
    protected float _elapsedTime;
    protected float _limitXright;
    protected float _limitXleft;
    protected float _limitYup;
    protected float _limitYdown;

    protected virtual void Start()
    {
        _moveDuration /= _speed;
        _startPosition = transform.position;
        _currentScale = transform.localScale;
        _scaleMultiplier = Random.Range(_minScale, _maxScale);

        StartCoroutine(Move());
    }

    protected IEnumerator Move()
    {
        //первый влет на экран
        _newScale = Vector3.one * _scaleMultiplier;
        _endPosition = new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), 0f);
        while (_elapsedTime < _firstMoveDuration)
        {
            _elapsedTime += Time.deltaTime;
            float percentageComplete = _elapsedTime / _moveDuration;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(percentageComplete));
            transform.localScale = Vector3.Lerp(_currentScale, _newScale, _curve.Evaluate(percentageComplete));
            yield return null;
        }

        //движение по экрану
        while (_movesCount > 0)
        {
            _elapsedTime = 0f;
            _startPosition = transform.position;
            _endPosition = new Vector3(Random.Range(_limitXleft, _limitXright), Random.Range(_limitYdown, _limitYup),
                0f);
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
            _endPosition = new Vector3(Random.Range(_limitXleft, _limitXright), Random.Range(_limitYdown, _limitYup),
                0f);
            _currentScale = transform.localScale;
            _scaleMultiplier = Random.Range(_minScale, _maxScale);
            _newScale = Vector3.one * _scaleMultiplier;
            _movesCount--;
        }

        //выпиливаем насекомое, если долго не съедают
        float suicideAngle = Random.Range(-5f, 185f);
        float x = Mathf.Cos(suicideAngle) * _suicideRadius;
        float y = Mathf.Sin(suicideAngle) * _suicideRadius;
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