using System;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Some1.Net
{
    public sealed class DuplexPipe : IDuplexPipe
    {
        private readonly Action _reset;

        public DuplexPipe(PipeReader input, PipeWriter output, Action reset)
        {
            Input = input;
            Output = output;
            _reset = reset;
        }

        public PipeReader Input { get; }

        public PipeWriter Output { get; }

        public void Reset()
        {
            try
            {
                _reset();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception on DuplexPipe.Reset. {ex}");
            }
        }

        public async Task CompleteAndResetAsync()
        {
            await Input.CompleteAsync().ConfigureAwait(false);
            await Output.CompleteAsync().ConfigureAwait(false);
            Reset();
        }

        public void CompleteAndReset()
        {
            Input.Complete();
            Output.Complete();
            Reset();
        }
    }
}
