using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public class ScreenLoader<TScreen> : IScreenLoader<TScreen>
        where TScreen : IScreen
    {
        private readonly TScreen _screen;

        public ScreenLoader(TScreen screen)
        {
            _screen = screen;
        }

        public UniTask<TScreen> LoadScreen(CancellationToken cancellationToken)
        {
            return UniTask.FromResult(_screen);
        }
    }
}