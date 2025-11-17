using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Mosquito : InsectBase
{
    [SerializeField] private float _moveDuration;
    
    [SerializeField] private float _moveDurationOffset;
    
    private Vector3 _currentDirection;
    private float _directionX;
    
    void Start()
    {
        
        _currentDirection = new Vector3(1f, Random.Range(-1f, 1f), 0);
        StartCoroutine(Move());
    }

    void Update()
    {
        
    }
    
    private IEnumerator Move()
    {
        while (true)
        {
            float duration =_moveDuration + Random.Range(0f, _moveDurationOffset);
            float currentSpeed = _speed + Random.Range(-2f, 2f);
            
            float timer = 0f;
            while (timer < duration)
            {
                transform.Translate(_currentDirection * currentSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            
            _currentDirection = new Vector3(Random.Range(-0.6f, 1.2f), Random.Range(-0.5f, 0.5f), 0);
        }
    }
}
