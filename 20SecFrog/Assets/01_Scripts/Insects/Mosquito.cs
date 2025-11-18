public class Mosquito : LinearMovingInsect
{
    protected override void Start()
    {
        _limitXleft = -2f;
        _limitXright = 2f;
        _limitYdown = -2f;
        _limitYup = 2f;
        base.Start();
    }
}