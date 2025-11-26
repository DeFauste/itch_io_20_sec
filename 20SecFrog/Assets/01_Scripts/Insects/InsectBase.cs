using UnityEngine;

public class InsectBase : MonoBehaviour
{
    [SerializeField] private float _score;
    
    [Header("Движение")]
    [SerializeField] protected float _speed = 1f;
    
    void Start()
    {
        
    }
    
    void Update()
    {
       
    }

    public float GetScore()
    {
        return _score;
    }
    
}
