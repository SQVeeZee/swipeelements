using System.Threading;
using UniRx;

namespace Project.Core.Utility
{
    public static class CompositeDisposableExtensions
    {
        public static CancellationToken GetOrCreateToken(this CompositeDisposable disposables)
        {
            foreach (var disposable in disposables)
            {
                if (disposable is DisposableCancellationToken token)
                {
                    return token.Token;
                }
            }

            var newToken = new DisposableCancellationToken();
            disposables.Add(newToken);
            return newToken.Token;
        }
    }
}