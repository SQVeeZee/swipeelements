using System;
using System.Threading;

namespace Project.Core.Utility
{
    public sealed class DisposableCancellationToken : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public CancellationToken Token => _cancellationTokenSource.Token;

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}