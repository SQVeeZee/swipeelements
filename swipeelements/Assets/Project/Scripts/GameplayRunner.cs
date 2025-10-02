using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Core.Runner;
using Zenject;

namespace Project
{
    public sealed class GameplayRunner : IModuleRunner
    {
        private DiContainer _container;
        private List<ISceneModule> _modules;
        private ICancellationToken _appCancellationToken;

        [Inject]
        private void Construct(
            [Inject(Id = ModuleCancellationToken.Id)] ICancellationToken moduleCancellationToken,
            DiContainer container)
        {
            _container = container;
        }

        public void RunModule()
        {
            ModulesInitialization(_appCancellationToken.Token).Forget();
        }

        public void Dispose()
        {
            foreach (var module in _modules)
            {
                module.Dispose();
            }
        }

        private async UniTask ModulesInitialization(CancellationToken token)
        {
            _modules = _container.ResolveAll<ISceneModule>();
            await UniTask.WhenAll(_modules.Select(m => m.InitializeAsync(token)))
                .AttachExternalCancellation(token);
        }

    }
}