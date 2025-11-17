using System.Collections;
using UnityEngine;

public class TheFly : InsectBase
{
    [SerializeField] private float _durationOfSpeed = 1f;
    
    private float _direction;
    private float _frequencyX;
    private float _frequencyY;
    private Vector3 _center;
    private float _radius;
    private float _offsetX;
    private float _offsetY;
    private float _sideSpeedMultiplier;
    private float _timer;

    private void Start()
    {
        SetDirection();

        _center = transform.position;
        _radius = Random.Range(1.2f, 2.2f);
        _offsetX = Random.Range(1.5f, 2.2f);
        _offsetY = Random.Range(0.4f, 0.8f);
        _frequencyX = Random.Range(1.67f, 2.55f);
        _frequencyY = Random.Range(2.22f, 3.66f);
        StartCoroutine(Move());
    }

    private void SetDirection()
    {
        _direction = transform.position.x > 0 ? -1 : 1;    
    }

    private IEnumerator Move()
    {
        float angle = 0f;
        _sideSpeedMultiplier = 1f;
        
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer >= _durationOfSpeed)
            {
                _timer = 0;
               _sideSpeedMultiplier = Random.Range(-0.5f, 2f);
            }
            angle += _speed * Time.deltaTime; 
            
            float x = Mathf.Cos(angle * _frequencyX) * _radius * _offsetX;
            float y = Mathf.Sin(angle * _frequencyY) * _radius * _offsetY;

            Vector3 newPosition = new Vector3(x, y, 0f);
            transform.position = _center + newPosition;
           _center += new Vector3(_speed * _sideSpeedMultiplier * _direction, 0f, 0f)  * Time.deltaTime;
           
           yield return null;
        } 
    }
}
