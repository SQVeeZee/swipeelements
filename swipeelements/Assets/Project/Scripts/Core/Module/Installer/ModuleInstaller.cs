using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core.Utility;
using Zenject;

namespace Project.Core
{
    public class ModuleInstaller<TModuleToken> : InstallerBase
        where TModuleToken : ModuleToken
    {
        private readonly TModuleToken _token;
        private readonly CancellationToken _cancellationToken;

        public ModuleInstaller(TModuleToken token, CancellationToken cancellationToken)
        {
            _token = token;
            _cancellationToken = cancellationToken;
        }

        public override void InstallBindings()
        {
            Container.Bind<ModuleToken>().FromInstance(_token).AsSingle();
            Container.Bind<TModuleToken>().FromInstance(_token).AsSingle();
            var moduleCancellationToken = new ModuleCancellationToken(_cancellationToken);
            Container.BindCancellationToken(moduleCancellationToken, ModuleCancellationToken.Id);
        }
    }

    public abstract class ModuleToken<TResult> : ModuleToken
    {
        public TResult Result { get; private set; }

        public void Finish(TResult result)
        {
            Finish(result, null, false);
        }

        public void Finish(TResult result, string nextHint)
        {
            Finish(result, nextHint, false);
        }

        public void Finish(TResult result, bool forceShowLoading)
        {
            Finish(result, null, forceShowLoading);
        }

        private void Finish(TResult result, string nextHint, bool forceShowLoading)
        {
            Result = result;
            Finish(nextHint, forceShowLoading);
        }
    }

    public abstract class ModuleToken
    {
        private readonly UniTaskCompletionSource<bool> _finished = new();
        public string NextHint { get; private set; }
        public bool ForceShowLoading { get; private set; }

        public async UniTask WaitForResultAsync(CancellationToken cancellationToken)
        {
            using var registration = cancellationToken.Register(() => _finished.TrySetCanceled());
            await _finished.Task;
        }

        protected void Finish(string nextHint = null, bool forceShowLoading = false)
        {
            NextHint = nextHint;
            ForceShowLoading = forceShowLoading;
            _finished.TrySetResult(true);
        }
    }

    public class GameplayToken : ModuleToken<GameplayToken.ModuleResult>
    {
        public enum ModuleResult
        {
            Resume,
            Revive,
            NextLevel,
            Restart,
            Menu,
            AutoComplete,
            Screenshot,
        }
    }
}