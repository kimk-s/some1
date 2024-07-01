using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Some1.Net
{
    public sealed class DuplexPipePair
    {
        private readonly object _sync = new();
        private readonly Pipe _pipe1;
        private readonly Pipe _pipe2;
        private bool _resetA;
        private bool _resetB;

        public DuplexPipePair()
        {
            const int PauseWriterThreshold = 65536 * 100;
            const int ResumeWriterThreshold = (int)(PauseWriterThreshold * 0.8f);

            var pipeoOptions = new PipeOptions(
                pauseWriterThreshold: PauseWriterThreshold,
                resumeWriterThreshold: ResumeWriterThreshold);

            _pipe1 = new Pipe(pipeoOptions);
            _pipe2 = new Pipe(pipeoOptions);
            A = new DuplexPipe(_pipe1.Reader, _pipe2.Writer, ResetA);
            B = new DuplexPipe(_pipe2.Reader, _pipe1.Writer, ResetB);
        }

        public DuplexPipe A { get; }

        public DuplexPipe B { get; }

        public async Task CompleteAndResetAsync()
        {
            await A.CompleteAndResetAsync().ConfigureAwait(false);
            await B.CompleteAndResetAsync().ConfigureAwait(false);
        }

        public void CompleteAndReset()
        {
            A.CompleteAndReset();
            B.CompleteAndReset();
        }

        private void ResetA()
        {
            lock (_sync)
            {
                if (_resetA)
                {
                    throw new InvalidOperationException();
                }

                _resetA = true;

                TryResetCore();
            }
        }

        private void ResetB()
        {
            lock (_sync)
            {
                if (_resetB)
                {
                    throw new InvalidOperationException();
                }

                _resetB = true;

                TryResetCore();
            }
        }

        private void TryResetCore()
        {
            if (!_resetA || !_resetB)
            {
                return;
            }

            _pipe1.Reset();
            _pipe2.Reset();
            _resetA = false;
            _resetB = false;
            DuplexPipePairPool.Return(this);
        }
    }
}
