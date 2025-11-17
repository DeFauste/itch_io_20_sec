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
    [SerializeField] private float _spawnDelay;
    
    private Quaternion _currentRotation;
    
    void Start()
    {
        SetRotation();
        StartCoroutine(Spawn());
    }

   
    void Update()
    {
        
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
        for (int i = 0; i < _count; i++)
        {
            Instantiate(_insectPrefab, transform.position, _currentRotation);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
