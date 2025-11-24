using UnityEngine;

public class Mosquito : LinearMovingInsect
{
    protected override void Start()
    {
        _moveMaxLimit = new Vector3 (2f, 2f, 0f);
        _moveMinLimit = new Vector3 (0.5f, 0.5f, 0f);
        base.Start();
    }
}