using System.Collections;
using MainMenu;
using UnityEngine;
using Random = UnityEngine.Random;

public class MayBug : InsectBase
{
    [SerializeField] private float _frequencyY;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _radius;
    [SerializeField] private float _sideSpeedMultiplier; 
    private GameState gameState;

    private float _direction;
    private Vector3 _center;

    private void Start()
    {
        gameState = GameState.Instance;
        SetDirection();
        //transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
        _center = transform.position;

        StartCoroutine(Move());
    }

    private void SetDirection()
    {
        _direction = transform.position.x > 0 ? -1 : 1;
    }

    private IEnumerator Move()
    {
        float angle = Random.Range(0f, 2 * Mathf.PI);

        while (true)
        {
            while (gameState.Paused)
                yield return null;
            
            angle += Time.deltaTime * 2 * Mathf.PI * _speed;

            float y = Mathf.Sin(angle * _frequencyY) * _radius * _offsetY;
            Vector3 newPosition = new Vector3(0f, y, 0f);
            transform.position = _center + newPosition;
            _center += new Vector3(_speed * _sideSpeedMultiplier * _direction, 0f, 0f) * Time.deltaTime;

            if (Mathf.Abs(transform.position.x) > 15f || Mathf.Abs(transform.position.y) > 15f)
            {
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
