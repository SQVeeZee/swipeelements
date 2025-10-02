using System.Threading;
using JetBrains.Annotations;

namespace Project.Core
{
    [UsedImplicitly]
    public class ModuleCancellationToken : BaseCancellationToken
    {
        public const string Id = "module_cancellation_token";

        public ModuleCancellationToken(CancellationToken cancellationToken)
            => Current = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    }
}