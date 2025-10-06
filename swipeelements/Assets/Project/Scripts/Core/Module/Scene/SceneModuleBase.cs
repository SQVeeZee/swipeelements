using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public abstract class SceneModuleBase : ISceneModule
    {
        UniTask ISceneModule.InitializeAsync(CancellationToken cancellationToken)
        {
            InitializeAsync(cancellationToken);
            return UniTask.CompletedTask;
        }

        void ISceneModule.Dispose() => Dispose();
        void ISceneModule.Tick() => Tick();

        protected virtual UniTask InitializeAsync(CancellationToken cancellationToken) => UniTask.CompletedTask;
        protected virtual void Tick() { }
        protected virtual void Dispose() { }
    }
}