using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MayBug : InsectBase
{
    private Vector3 _currentDirection;
    private float _offsetY = 1.6f;
    void Start()
    {
        _speed += Random.Range(-0.7f, 0.7f);
        _currentDirection = Vector3.right;
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        float angle = Random.Range(0f, 2 * Mathf.PI);
        while (true)
        {
            angle += Time.deltaTime;
            _currentDirection = new Vector3(1f, Mathf.Cos(angle) * _offsetY, 0f);
            transform.Translate(_currentDirection * _speed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x) > 15f || Mathf.Abs(transform.position.y) > 15f)
            {
                gameObject.SetActive(false);
            }
            
            yield return null;
          
        }
    }
}
