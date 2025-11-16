using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Directions _spawnDirection;
    [SerializeField] private GameObject _insectPrefab;
    [SerializeField] private float _count;
    [SerializeField] private float _spawnDelay;
    
    private Quaternion _currentDirection;
    // Start is called before the first frame update
    void Start()
    {
        SetDirection();
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDirection()
    {
        if (_spawnDirection == Directions.Right)
        {
            _currentDirection = Quaternion.Euler(0, 0, 0);
        }
        else if (_spawnDirection == Directions.Left)
        {
            _currentDirection = Quaternion.Euler(0, 180, 0);
        }
    }
    
    private IEnumerator Spawn()
    {
        for (int i = 0; i < _count; i++)
        {
            Instantiate(_insectPrefab, transform.position, _currentDirection);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
