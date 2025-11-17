using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBase : MonoBehaviour
{
    [SerializeField] private float _score;
    
    [Header("Движение")]
    [SerializeField] protected float _speed;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        //transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    public float GetScore()
    {
        return _score;
    }
    
}
