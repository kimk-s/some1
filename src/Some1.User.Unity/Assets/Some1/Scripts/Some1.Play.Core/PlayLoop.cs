#if !UNITY
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Some1.Play.Core.Options;

namespace Some1.Play.Core
{
    public sealed class PlayLoop
    {
        private readonly ILogger<PlayLoop> _logger;
        private readonly PlayLoopOptions _options;
        private readonly IPlayCore _core;

        public PlayLoop(
            ILogger<PlayLoop> logger,
            IOptions<PlayLoopOptions> options,
            IPlayCore core)
        {
            _logger = logger;
            _options = options.Value;
            _core = core;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("FPS: {FPS}, MinFPS: {MinFps}, TimeScale: {TimeScale}, LogOnOverRate: {LogOnOverRate}", _options.FPS, _options.MinFPS, _options.TimeScale, _options.LogOnOverRate);

            float frameSeconds = 1f / _options.FPS;
            float maxFrameSeconds = 1f / Math.Min(_options.MinFPS, _options.FPS);
            float timeScale = _options.TimeScale;
            float logOnOverRate = Math.Max(_options.LogOnOverRate, 0);
            using var looper = new LogicLooper(_options.FPS);

            await looper.RegisterActionAsync((in LogicLooperActionContext ctx) =>
            {
                float elapsed = (float)ctx.ElapsedTimeFromPreviousFrame.TotalSeconds;

                //if (elapsed > (frameSeconds * (1 + logOnOverRate)))
                //{
                //    _logger.LogWarning($"Over frame time {elapsed:N3}/{frameSeconds:N3}s");
                //}

                float deltaSeconds = Math.Min(elapsed, maxFrameSeconds);

                _core.Update(deltaSeconds * timeScale);

                return !cancellationToken.IsCancellationRequested && !ctx.CancellationToken.IsCancellationRequested;
            });
        }
    }
}
#endif
