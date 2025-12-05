using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Directions _spawnRotation;
    [SerializeField] private GameObject _insectPrefab;
    [SerializeField] private float _count;
    [SerializeField] private float _firstSpawnDelay;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private float _spawnOffsetY;
    
    private Quaternion _currentRotation;
    
    void Start()
    {
        SetRotation();
        StartCoroutine(Spawn());
    }
    private void SetRotation()
    {
        if (_spawnRotation == Directions.Right)
        {
            _currentRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_spawnRotation == Directions.Left)
        {
            _currentRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_firstSpawnDelay);
        
        //для тестов спаунер с количеством, потом выпилить
        for (int i = 0; i < _count; i++)
        {
            var y = Random.Range(-_spawnOffsetY, _spawnOffsetY);
            var x = transform.position.x;
            var spawnPosition = new Vector3(x, y , 0f);
            Instantiate(_insectPrefab, spawnPosition, _currentRotation, gameObject.transform);
            var spawnTime = Random.Range(_minSpawnTime, _maxSpawnTime); 
            yield return new WaitForSeconds(spawnTime);
        }
        
        //основной спаунер
        
        // var elapsedTime = 0f;
        // while (elapsedTime < 21f)
        // {
        //     Instantiate(_insectPrefab, transform.position, _currentRotation);
        //     var spawnTime = Random.Range(_minSpawnTime, _maxSpawnTime); 
        //     yield return new WaitForSeconds(spawnTime);
        //     
        //     elapsedTime += Time.deltaTime;
        // }
        // gameObject.SetActive(false);
    }
}
