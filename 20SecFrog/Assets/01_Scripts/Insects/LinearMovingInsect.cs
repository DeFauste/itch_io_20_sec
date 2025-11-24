using System.Collections;
using UnityEngine;

public class LinearMovingInsect : InsectBase
{
    [SerializeField] protected float _moveDuration;
    [SerializeField] protected int _movesCount;
    //[SerializeField] protected float _minScale;
    //[SerializeField] protected float _maxScale;
    [SerializeField] protected float _firstMoveDuration;
    [SerializeField] protected AnimationCurve _curve;

    [Header("Ограничения под size камеры")]
    [SerializeField] protected Vector3 _moveMinLimit = new(-9f, -3f, 0f);
    [SerializeField] protected Vector3 _moveMaxLimit = new(9f, 4f, 0f);
    [SerializeField] protected float _suicideTime = 2f;
    [SerializeField] protected float _suicideRadius = 12f;

    //protected Vector3 _currentScale;
    //protected Vector3 _newScale;
    //protected float _scaleMultiplier;
    
    protected virtual void Start()
    {
        _moveDuration /= _speed;
        
        //_currentScale = transform.localScale;
        //_scaleMultiplier = Random.Range(_minScale, _maxScale);

        StartCoroutine(Move());
    }

    protected IEnumerator Move()
    {
        //первый влет на экран
        //_newScale = Vector3.one * _scaleMultiplier;
        Vector3 firstPoint = GetRandomPointInRange(_moveMinLimit, _moveMaxLimit);
        yield return MoveTo(firstPoint, _firstMoveDuration);

        //движение по экрану
        int movesCount = 0;
        while (movesCount < _movesCount)
        {
            Vector3 endPoint = GetRandomPointInRange(_moveMinLimit, _moveMaxLimit);
            yield return MoveTo(endPoint, _moveDuration);

            movesCount++;
        }

        //выпиливаем насекомое, если долго не съедают
        float suicideAngle = Random.Range(-5f, 185f);
        float x = Mathf.Cos(suicideAngle) * _suicideRadius;
        float y = Mathf.Sin(suicideAngle) * _suicideRadius;
        Vector3 suicidePoint = new Vector3(x, y, 0f);

        yield return MoveTo(suicidePoint, _suicideTime);

        gameObject.SetActive(false);
    }


    private IEnumerator MoveTo(Vector3 endPoint, float duration)
    {
        Vector3 startPoint = transform.position;
        int rotation = endPoint.x > startPoint.x ? 0 : 180;
        transform.rotation = Quaternion.Euler(0f, rotation, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, _curve.Evaluate(percentageComplete));
            transform.position = newPosition;
            yield return null;
        }
    }

    private Vector3 GetRandomPointInRange(Vector3 min, Vector3 max)
    {
        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            0f
        );
    }
}