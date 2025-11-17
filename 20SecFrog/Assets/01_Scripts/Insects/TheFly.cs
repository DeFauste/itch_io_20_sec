using System.Collections;
using UnityEngine;

public class TheFly : InsectBase
{
    private float _direction;
    private float _frequencyX;
    private float _frequencyY;
    private Vector3 _center;
    private float _radius;
    private float _offsetX;
    private float _offsetY;
    private float _sideSpeedMultiplier;
    private void Start()
    {
        SetDirection();

        _center = transform.position;
        _radius = -Random.Range(1.2f, 2.6f);
        _offsetX = Random.Range(0.5f, 2.5f);
        _offsetY = Random.Range(0.6f, 1.1f);
        _frequencyX = Random.Range(1.6f, 2.5f);
        _frequencyY = Random.Range(2.2f, 3.6f);
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
            angle += Time.deltaTime;
            
            float x = Mathf.Cos(angle * _frequencyX) * _radius * _offsetX;
            float y = Mathf.Sin(angle * _frequencyY) * _radius * _offsetY;

            Vector3 newPosition = new Vector3(x, y, 0f);
            transform.position = _center + newPosition;
           _center += new Vector3(_speed * _sideSpeedMultiplier * _direction, 0f, 0f)  * Time.deltaTime;
           
           yield return null;
        } 
    }
}
