public class Dragonfly : LinearMovingInsect
{
    void Start()
    {
        _limitXleft = _minX;
        _limitXright = _maxX;
        _limitYdown = _minY;
        _limitYup = _maxY;
        base.Start();
    }
}