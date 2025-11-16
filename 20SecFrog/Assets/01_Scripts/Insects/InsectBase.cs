using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBase : MonoBehaviour
{
    [SerializeField] protected float _speed;

    [SerializeField] private float _score;
    
    
    void Start()
    {
        
    }
    
    void Update()
    {
        //transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }
    
}
