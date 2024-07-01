namespace Some1.Play.Core.TestHelpers;

public sealed class FakeClock : IClock
{
    private readonly DateTime _start;
    private readonly TimeSpan _delta;
    private DateTime _utcNow;

    public FakeClock() : this(new(2024, 1, 1), default)
    {
    }

    public FakeClock(DateTime start, TimeSpan delta)
    {
        _start = start;
        _delta = delta;
    }

    public DateTime UtcNow
    {
        get
        {
            if (_utcNow == default)
            {
                _utcNow = _start;
            }
            else
            {
                _utcNow = _utcNow.Add(_delta);
            }
            return _utcNow;
        }
    }

    public DateTime FakeUtcNow => _utcNow;
}
