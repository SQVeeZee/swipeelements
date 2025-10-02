using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Core.Utility;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project
{
    public sealed class ProjectRunner
    {
        private const string MenuScene = "Menu";
        private const string GameScene = "Gameplay";

        private readonly SceneService _sceneService;
        private readonly ILoadingProcessor _loadingProcessor;
        private readonly ICancellationToken _appCancellationToken;

        [Inject]
        private ProjectRunner(
            SceneService sceneService,
            ILoadingProcessor loadingProcessor,
            [Inject(Id = AppCancellationToken.Id)] ICancellationToken appCancellationToken)
        {
            _sceneService = sceneService;
            _loadingProcessor = loadingProcessor;
            _appCancellationToken = appCancellationToken;
        }

        public void Run() => RunGameplay();

        private void RunGameplay() => RunAsync(new GameplayToken(), _appCancellationToken.Token).Forget();

        private async UniTask RunAsync<TModuleToken>(TModuleToken moduleToken, CancellationToken cancellationToken)
            where TModuleToken : ModuleToken
        {
            await _sceneService.LoadAsync(GameScene, true, cancellationToken, _loadingProcessor);
            var scene = SceneManager.GetSceneByName(GameScene);
            var component = scene.FindFirstComponentOfType<SceneContext>();
            if (component == null)
            {
                throw new Exception($"Can't find component of type {nameof(SceneContext)} in scene {GameScene}");
            }
            var installer = new ModuleInstaller<TModuleToken>(moduleToken, cancellationToken);
            component.AddNormalInstaller(installer);
        }
    }
}
