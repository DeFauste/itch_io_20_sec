public class Dragonfly : LinearMovingInsect
{
    void Start()
    {
        _screenMoveMax = _moveMaxLimit;
        _screenMoveMin = _moveMinLimit;
        base.Start();
    }
}