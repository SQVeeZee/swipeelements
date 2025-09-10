using System.Threading;

namespace Project.Core
{
    public abstract class BaseCancellationToken : ICancellationToken, ICancellationTokenControl
    {
        private CancellationTokenSource _cts;
        public CancellationToken Token => _cts?.Token ?? CancellationToken.None;

        public CancellationToken GetOrCreateToken()
        {
            if (_cts is { IsCancellationRequested: false })
            {
                return _cts.Token;
            }
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            return _cts.Token;
        }

        public void Cancel()
        {
            if (_cts == null || _cts.IsCancellationRequested)
            {
                return;
            }

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }


        public void Reset()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}