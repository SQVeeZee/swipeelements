using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public class Service : IService
    {
        public virtual UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}