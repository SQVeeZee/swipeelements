using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Core.Runner;
using Zenject;

namespace Project.Gameplay
{
    public sealed class GameplayRunner : IModuleRunner, ITickable
    {
        private DiContainer _container;
        private List<ISceneModule> _modules;
        private ICancellationToken _moduleCancellationToken;

        [Inject]
        private void Construct(
            [Inject(Id = ModuleCancellationToken.Id)] ICancellationToken moduleCancellationToken,
            DiContainer container)
        {
            _moduleCancellationToken = moduleCancellationToken;
            _container = container;
        }

        void IModuleRunner.RunModule() => ModulesInitialization(_moduleCancellationToken.Token).Forget();
        void ITickable.Tick() => _modules.ForEach(module => module.Tick());

        void IModuleRunner.Dispose()
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