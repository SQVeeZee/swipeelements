using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface IService
    {
        UniTask InitializeAsync(CancellationToken cancellationToken);
    }
}