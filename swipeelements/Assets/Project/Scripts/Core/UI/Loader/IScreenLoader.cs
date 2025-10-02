using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface IScreenLoader<TScreen>
        where TScreen : IScreen
    {
        UniTask<TScreen> LoadScreen(CancellationToken cancellationToken);
    }
}