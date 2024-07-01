
// -----------------------------------------------------------------------------
// Old Version 2
// -----------------------------------------------------------------------------
//public sealed class WithServerPlayStreaming : IPlayStreaming
//{
//    private const float JitterDelay = 0.1f;
//    private const float ScaleFactor = 5;
//    private readonly ILogger<WithServerPlayStreaming> _logger;
//    private ITimeFront _time = null!;
//    private ReadDelegate _read = null!;
//    private GetReadableCountDelegate _getReadableCount = null!;
//    private InterpolateDelegate _interpolate = null!;
//    private float _a;
//    private float _b;
//    private float _x;

//    public WithServerPlayStreaming(ILogger<WithServerPlayStreaming> logger)
//    {
//        _logger = logger;
//    }

//    public void Setup(ITimeFront time, ReadDelegate read, GetReadableCountDelegate getReadableCount, InterpolateDelegate interpolate)
//    {
//        _time = time;
//        _read = read;
//        _getReadableCount = getReadableCount;
//        _interpolate = interpolate;
//    }

//    public void Update(float deltaSeconds)
//    {
//        float interpolateTime = Read(deltaSeconds);
//        _interpolate(interpolateTime);
//    }

//    public void ResetState()
//    {
//        _a = 0;
//        _b = 0;
//        _x = 0;
//    }

//    private float Read(float deltaSeconds)
//    {
//        while (_a == 0)
//        {
//            if (TryRead())
//            {
//                _x = _a;
//            }
//            else
//            {
//                break;
//            }
//        }

//        if (_a > 0)
//        {
//            float delay = GetReadableCount() * GetDelta();

//            float diff = delay - JitterDelay;
//            //float scale = Math.Clamp(1 + (diff < 0 ? -1 : 1) * MathF.Pow(diff * ScaleFactor, 2), 0.1f, 10f);
//            float scale = Math.Clamp(1 + diff * ScaleFactor, 0.1f, 10f);
//            _x += deltaSeconds * scale;

//            while (_b <= _x)
//            {
//                if (!TryRead())
//                {
//                    _logger.LogInformation($"------------ x ------------");
//                    ResetState();
//                    break;
//                }
//            }
//        }

//        float t = _a == 0 ? 1 : (_x - _a) / (_b - _a);
//        Debug.Assert(t >= 0 && t <= 1);
//        return t;
//    }

//    private bool TryRead()
//    {
//        if (_read())
//        {
//            _a = _b;
//            _b = _time.TotalSeconds.Value.B;
//            Debug.Assert(_a < _b);
//            return true;
//        }
//        return false;
//    }

//    private int GetReadableCount()
//    {
//        return _getReadableCount(100);
//    }

//    private float GetDelta()
//    {
//        if (_a == 0 || _a == _b)
//        {
//            throw new InvalidOperationException();
//        }
//        return _b - _a;
//    }
//}

// -----------------------------------------------------------------------------
// Old Version 1
// -----------------------------------------------------------------------------
//public sealed class RemotePlayUpdater : IPlayUpdater
//{
//    private const float JitterDelay = 0.1f;
//    private const float ScaleFactor = 5;
//    private readonly ILogger<RemotePlayUpdater> _logger;
//    private ITimeFront _time = null!;
//    private float _a;
//    private float _b;
//    private float _x;

//    public RemotePlayUpdater(ILogger<RemotePlayUpdater> logger)
//    {
//        _logger = logger;
//    }

//    public void Setup(ITimeFront time)
//    {
//        _time = time;
//    }

//    public float Update(float deltaSeconds, Syncer<RequestSyncObject, ResponseSyncObject> syncer)
//    {
//        float b = _b;
//        float result = Read(deltaSeconds, syncer);
//        if (b != _b)
//        {
//            Write(syncer);
//        }
//        return result;
//    }

//    private static void Write(SyncerWriter<RequestSyncObject> writer)
//    {
//        writer.TryWrite();
//    }

//    private float Read(float deltaSeconds, SyncerReader<ResponseSyncObject> reader)
//    {
//        if (reader.Number == 0)
//        {
//            ResetRead();
//        }

//        while (_a == 0)
//        {
//            if (TryRead(reader))
//            {
//                _x = _a;
//            }
//            else
//            {
//                break;
//            }
//        }

//        if (_a > 0)
//        {
//            float delay = reader.Count * GetDelta();

//            float diff = delay - JitterDelay;
//            //float scale = Math.Clamp(1 + (diff < 0 ? -1 : 1) * MathF.Pow(diff * ScaleFactor, 2), 0.1f, 10f);
//            float scale = Math.Clamp(1 + diff * ScaleFactor, 0.1f, 10f);
//            _x += deltaSeconds * scale;

//            while (_b <= _x)
//            {
//                if (!TryRead(reader))
//                {
//                    _logger.LogInformation($"------------ x ------------");
//                    ResetRead();
//                    break;
//                }
//            }
//        }

//        float t = _a == 0 ? 1 : (_x - _a) / (_b - _a);
//        Debug.Assert(t >= 0 && t <= 1);
//        return t;
//    }

//    private bool TryRead(SyncerReader<ResponseSyncObject> reader)
//    {
//        if (reader.TryRead())
//        {
//            _a = _b;
//            _b = _time.TotalSeconds.Value.B;
//            Debug.Assert(_a < _b);
//            return true;
//        }
//        return false;
//    }

//    private void ResetRead()
//    {
//        _a = 0;
//        _b = 0;
//        _x = 0;
//    }

//    private float GetDelta()
//    {
//        if (_a == 0 || _a == _b)
//        {
//            throw new InvalidOperationException();
//        }
//        return _b - _a;
//    }
//}
