using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public abstract class Service : IService
    {
        UniTask IService.InitializeServiceAsync(CancellationToken cancellationToken) => InitializeAsync(cancellationToken);

        protected abstract UniTask InitializeAsync(CancellationToken cancellationToken);

        public virtual void Dispose() { }
    }
}