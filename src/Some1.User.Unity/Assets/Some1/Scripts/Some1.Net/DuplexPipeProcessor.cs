using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Net
{
    public sealed class DuplexPipeProcessor
    {
        public async Task ProcessAsync(Socket socket, DuplexPipe pipe, CancellationToken cancellationToken)
        {
            await Task.WhenAll(
                SendAsync(socket, pipe.Input, cancellationToken),
                ReceiveAsync(socket, pipe.Output, cancellationToken)).ConfigureAwait(false);

            socket.Close();
            pipe.Reset();
        }

        private async Task SendAsync(Socket socket, PipeReader input, CancellationToken cancellationToken)
        {
            await Task.Yield();

            List<ArraySegment<byte>>? bufferList = null;
            Exception? exception = null;

            try
            {
                while (true)
                {
                    ReadResult result = await input.ReadAsync(cancellationToken).ConfigureAwait(false);
                    ReadOnlySequence<byte> buffer = result.Buffer;

                    if (buffer.IsSingleSegment)
                    {
                        if (buffer.FirstSpan.Length > 0)
                        {
                            await socket.SendAsync(buffer.First, SocketFlags.None, cancellationToken).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        if (bufferList is null)
                        {
                            bufferList = new();
                        }
                        else
                        {
                            bufferList.Clear();
                        }

                        foreach (var item in buffer)
                        {
                            if (!MemoryMarshal.TryGetArray(item, out var segment))
                            {
                                throw new InvalidOperationException();
                            }
                            bufferList.Add(segment);
                        }
                        await socket.SendAsync(bufferList, SocketFlags.None).ConfigureAwait(false);
                    }

                    input.AdvanceTo(buffer.End);

                    if (result.IsCompleted)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            catch
            {
            }

            await input.CompleteAsync(exception).ConfigureAwait(false);
        }

        private async Task ReceiveAsync(Socket socket, PipeWriter output, CancellationToken cancellationToken)
        {
            const int minimumBufferSize = 512;
            Exception? exception = null;

            try
            {
                while (true)
                {
                    Memory<byte> memory = output.GetMemory(minimumBufferSize);
#if UNITY
                    if (!MemoryMarshal.TryGetArray<byte>(memory, out var segment))
                    {
                        throw new InvalidOperationException();
                    }
                    int bytesRead = await socket.ReceiveAsync(segment, SocketFlags.None).ConfigureAwait(false);
#else
                    int bytesRead = await socket.ReceiveAsync(memory, cancellationToken).ConfigureAwait(false);
#endif
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    output.Advance(bytesRead);

                    FlushResult result = await output.FlushAsync(cancellationToken).ConfigureAwait(false);

                    if (result.IsCompleted)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                socket.Shutdown(SocketShutdown.Receive);
            }
            catch
            {
            }

            await output.CompleteAsync(exception).ConfigureAwait(false);
        }
    }
}
