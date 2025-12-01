using UnityEngine;

public class Mosquito : LinearMovingInsect
{
    protected override void Start()
    {
        _screenMoveMax = new Vector3(2f, 2f, 0f);
        _screenMoveMin = new Vector3(-2f, -2f, 0f);
        base.Start();
    }
}