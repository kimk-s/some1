using System.Collections.Concurrent;
using System.Diagnostics;
using Cysharp.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Some1.Play.Client.Tcp;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.CLI;

public class Command : ConsoleAppBase
{
    private readonly ILogger<Command> _logger;
    private readonly IPlayAddressGettable _addressGettable;
    private readonly IServiceProvider _services;

    private int _startingCount;
    private int _processingCount;
    private int _finishedCount;
    private int _terminatedCount;
    private int _faultedCount;

    public Command(
        ILogger<Command> logger,
        IPlayAddressGettable addressGettable,
        IServiceProvider services)
    {
        _logger = logger;
        _addressGettable = addressGettable;
        _services = services;
    }

    [Command("run")]
    public async Task RunAsync(string address, int port, int count = 1, float time = 1, int fps = 30)
    {
        _logger.LogInformation($"Run started. {GetOptionString(nameof(address), address)}, {GetOptionString(nameof(port), port)}, {GetOptionString(nameof(count), count)}, {GetOptionString(nameof(time), time)}");
        static string GetOptionString<T>(string key, T value) => $"{key}: {value}";

        ((PlayAddress)_addressGettable).SetAddress($"{address}:{port}");

        var states = Enumerable.Range(0, count)
            .Select(_ => new PlayState(_services.CreateScope()))
            .ToArray();

        using var looper = new LogicLooper(fps);
        var stopwatch = Stopwatch.StartNew();

        await Task.WhenAll(
            looper.RegisterActionAsync((in LogicLooperActionContext ctx) =>
            {
                var context = ctx;
                bool timeout = stopwatch.Elapsed.TotalSeconds > time;

                try
                {
                    Parallel.ForEach(
                        Partitioner.Create(0, states.Length),
                        new ParallelOptions()
                        {
                            CancellationToken = ctx.CancellationToken
                        },
                        source =>
                        {
                            foreach (var state in states.AsSpan(source.Item1, source.Item2 - source.Item1))
                            {
                                state.Update(context, timeout, _logger);
                            }
                        });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                    return true;
                }

                return !ctx.CancellationToken.IsCancellationRequested && (!timeout || PlayState.s_startingCount != PlayState.StoppedCount);
            }),
            looper.RegisterActionAsync((in LogicLooperActionContext ctx) =>
            {
                LogPlayStateIfChanged();
                bool timeout = stopwatch.Elapsed.TotalSeconds > time;
                return !ctx.CancellationToken.IsCancellationRequested && !timeout;
            }, LooperActionOptions.Default with { TargetFrameRateOverride = 1 }));

        foreach (var item in states)
        {
            item.Dispose();
        }

        LogPlayState();

        _logger.LogInformation($"Run stopped.");
    }

    private void LogPlayStateIfChanged()
    {
        if (_startingCount != PlayState.s_startingCount
            || _processingCount != PlayState.s_processingCount
            || _finishedCount != PlayState.s_finishedCount
            || _terminatedCount != PlayState.s_terminatedCount
            || _faultedCount != PlayState.s_faultedCount)
        {
            LogPlayState();
        }
    }

    private void LogPlayState()
    {
        _startingCount = PlayState.s_startingCount;
        _processingCount = PlayState.s_processingCount;
        _finishedCount = PlayState.s_finishedCount;
        _terminatedCount = PlayState.s_terminatedCount;
        _faultedCount = PlayState.s_faultedCount;

        _logger.LogInformation($"Starting: {_startingCount}, Processing: {_processingCount}, Finished: {_finishedCount}, Terminated: {_terminatedCount}, Faulted: {_faultedCount}");
    }

    private sealed class PlayState : IDisposable
    {
        public static int s_startingCount;
        public static int s_processingCount;
        public static int s_finishedCount;
        public static int s_terminatedCount;
        public static int s_faultedCount;

        public static int StoppedCount => s_finishedCount + s_terminatedCount + s_faultedCount;

        private readonly IServiceScope _scope;
        private readonly IPlayFront _front;
        private bool _starting;
        private bool _processing;
        private bool _finishied;
        private bool _terminated;
        private bool _faulted;

        public PlayState(IServiceScope scope)
        {
            _scope = scope;
            _front = scope.ServiceProvider.GetRequiredService<IPlayFront>();
        }

        private void SetStarting()
        {
            if (_starting) return;
            _starting = true;
            Interlocked.Increment(ref s_startingCount);
        }

        private void SetProcessing()
        {
            if (_processing) return;
            _processing = true;
            Interlocked.Increment(ref s_processingCount);
        }

        private void SetFinished()
        {
            if (_finishied) return;
            _finishied = true;
            Interlocked.Increment(ref s_finishedCount);
        }

        private void SetTerminated()
        {
            if (_terminated) return;
            _terminated = true;
            Interlocked.Increment(ref s_terminatedCount);
        }

        private void SetFaulted()
        {
            if (_faulted) return;
            _faulted = true;
            Interlocked.Increment(ref s_faultedCount);
        }

        public void Update(in LogicLooperActionContext context, bool timeout, ILogger<Command> _logger)
        {
            try
            {
                switch (_front.PipeState.CurrentValue.Status)
                {
                    case PipeStatus.None:
                        {
                            if (timeout)
                            {
                                break;
                            }

                            SetStarting();
                            _front.StartPipeAsync(context.CancellationToken);
                        }
                        break;
                    case PipeStatus.Starting:
                        break;
                    case PipeStatus.Processing:
                        {
                            SetProcessing();

                            if (timeout)
                            {
                                _front.FinishPipe();
                                break;
                            }

                            float deltaSeconds = (float)context.ElapsedTimeFromPreviousFrame.TotalSeconds;
                            if (deltaSeconds <= 0)
                            {
                                break;
                            }

                            _front.Walk(new(true, 360 * Random.Shared.NextSingle()));
                            _front.Cast(new((CastId)Random.Shared.Next(3), (short)Random.Shared.Next(255), false, Aim.Auto));

                            if (_front.Player.Object.Region.Section.CurrentValue?.Type == SectionType.Town
                                && _front.IsUnaryRunning.CurrentValue == false)
                            {
                                _front.ReadyGameAsync(context.CancellationToken);
                            }

                            _front.UpdatePipe(deltaSeconds);
                        }
                        break;
                    case PipeStatus.Finishing:
                        {
                            float deltaSeconds = (float)context.ElapsedTimeFromPreviousFrame.TotalSeconds;
                            if (deltaSeconds <= 0)
                            {
                                break;
                            }

                            _front.UpdatePipe(deltaSeconds);
                        }
                        break;
                    case PipeStatus.Finished:
                        {
                            SetFinished();
                        }
                        break;
                    case PipeStatus.Terminated:
                        {
                            SetTerminated();
                        }
                        break;
                    case PipeStatus.Faulted:
                        {
                            SetFaulted();
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
            }
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
