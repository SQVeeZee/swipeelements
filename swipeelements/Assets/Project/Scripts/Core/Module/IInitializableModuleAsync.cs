using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface IInitializableModuleAsync
    {
        UniTask InitializeAsync(CancellationToken cancellationToken);
    }
}