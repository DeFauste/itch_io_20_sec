using UnityEngine;

public class InsectBase : MonoBehaviour
{
    [SerializeField] private InsectType _insectType;
    [SerializeField] private float _score;

    [Header("Движение")] [SerializeField] protected float _speed = 1f;

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

    public InsectType GetInsectType()
    {
        return _insectType;
    }
}