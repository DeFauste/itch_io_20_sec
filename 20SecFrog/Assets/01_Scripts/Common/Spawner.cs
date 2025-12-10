using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Directions _spawnRotation;
    [SerializeField] private GameObject _insectPrefab;
    [SerializeField] private float _firstSpawnDelay;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private float _spawnOffsetY;

    private Quaternion _currentRotation;
    private Coroutine _coroutine;

    public void StartSpawning()
    {
        SetRotation();
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Spawn());
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

        var time = 20f - _firstSpawnDelay;
        var elapsedTime = 0f;
        while (elapsedTime < time)
        {
            var offsetY = Random.Range(-_spawnOffsetY, _spawnOffsetY);
            var spawnPoint = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            Instantiate(_insectPrefab, spawnPoint, _currentRotation, transform);
            var spawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            elapsedTime += spawnTime;
        }
    }
}