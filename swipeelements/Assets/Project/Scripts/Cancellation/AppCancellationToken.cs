using System.Threading;
using JetBrains.Annotations;

namespace Project.Core
{
    [UsedImplicitly]
    public class AppCancellationToken : BaseCancellationToken
    {
        public const string Id = "app_cancellation_token";

        public AppCancellationToken(CancellationToken cancellationToken)
            => Current = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    }
}