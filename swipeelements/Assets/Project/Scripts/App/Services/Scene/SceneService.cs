using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

namespace Project.Core
{
    public sealed class SceneService : Service, IDisposable
    {
        private readonly SemaphoreSlim _gate = new(1, 1);

        protected override UniTask InitializeAsync(CancellationToken cancellationToken) => UniTask.CompletedTask;
        public override void Dispose() => _gate.Dispose();

        public async UniTask LoadAsync(string sceneName, bool setActive, CancellationToken cancellationToken, ILoadingProcessor loadingProcessor = null)
        {
            await _gate.WaitAsync(cancellationToken);
            try
            {
                if (loadingProcessor != null)
                {
                    await loadingProcessor.ShowAsync(cancellationToken);
                }

                var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                op.allowSceneActivation = false;

                while (op.progress < 0.9f)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    loadingProcessor?.SetProgress(op.progress);
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }

                loadingProcessor?.SetProgress(1f);
                op.allowSceneActivation = true;

                while (!op.isDone)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }

                var loaded = SceneManager.GetSceneByName(sceneName);
                if (setActive && loaded.IsValid())
                {
                    SceneManager.SetActiveScene(loaded);
                }

                if (loadingProcessor != null)
                {
                    await loadingProcessor.HideAsync(cancellationToken);
                }
                await UniTask.Yield();
            }
            finally
            {
                if (_gate.CurrentCount == 0)
                {
                    _gate.Release();
                }
            }
        }

        public async UniTask UnloadAsync(string sceneName, CancellationToken token = default)
        {
            await _gate.WaitAsync(token);
            try
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid())
                {
                    return;
                }

                var op = SceneManager.UnloadSceneAsync(scene);
                if (op == null)
                {
                    return;
                }

                while (!op.isDone)
                {
                    token.ThrowIfCancellationRequested();
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            finally
            {
                if (_gate.CurrentCount == 0)
                {
                    _gate.Release();
                }
            }
        }
    }
}