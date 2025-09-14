using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface IService
    {
        UniTask InitializeServiceAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}