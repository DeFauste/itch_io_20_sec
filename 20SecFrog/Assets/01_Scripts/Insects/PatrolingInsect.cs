using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingInsect : InsectBase
{
    [SerializeField] private Vector3 _moveMinLimit = new(-9f, -3f, 0f);
    [SerializeField] private Vector3 _moveMaxLimit = new(9f, 4f, 0f);
    [SerializeField] private float _moveDuration = 3f;
    [SerializeField] private int _movesCount = 4;
    [SerializeField] private AnimationCurve _curve;

    [SerializeField] private Vector3 _patrolLimits = new(1f, 1f, 0f);
    [SerializeField] private float _patrolDuration = 0.5f;
    [SerializeField] private int _patrolCount = 4;

    [SerializeField] protected float _suicideTime = 2f;
    [SerializeField] protected float _suicideRadius = 12f;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        // летит
        int movesCount = 0;
        while (movesCount < _movesCount)
        {
            Vector3 endPoint = GetRandomPointInRange(_moveMinLimit, _moveMaxLimit);
            yield return MoveTo(endPoint, _moveDuration);

            Vector3 patrolCenter = transform.position;
            // патрулирует
            for (int i = 0; i < _patrolCount; i++)
            {
                Vector3 patrolPoint = GetRandomShortTargetAround(patrolCenter, _patrolLimits);
                yield return MoveTo(patrolPoint, _patrolDuration);
            }

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

    private Vector3 GetRandomShortTargetAround(Vector3 center, Vector3 limits)
    {
        Vector3 offset = new Vector3(
            Random.Range(-limits.x, limits.x),
            Random.Range(-limits.y, limits.y),
            0f
        );
        return center + offset;
    }
}
