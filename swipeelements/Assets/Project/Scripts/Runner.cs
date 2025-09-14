using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using UnityEngine;
using Zenject;

public class Runner : MonoBehaviour
{
    private const int DefaultFPS = 60;

    [SerializeField]
    private SceneContext _sceneContext;

    private readonly CancellationTokenSource _initializeTokenSource = new();
    private List<IService> _services;

    private void Awake() => RunAsync(_initializeTokenSource.Token).Forget();

    private async UniTaskVoid RunAsync(CancellationToken cancellationToken)
    {
        SetApplicationFramerate();

        _sceneContext.Run();
        await InitializeServices(cancellationToken);
        await ModulesInitialization(cancellationToken);
    }

    private async UniTask InitializeServices(CancellationToken cancellationToken)
    {
        _services = _sceneContext.Container.ResolveAll<IService>();
        await UniTask.WhenAll(_services.Select(i => i.InitializeServiceAsync(cancellationToken))).AttachExternalCancellation(cancellationToken);
    }

    private async UniTask ModulesInitialization(CancellationToken cancellationToken)
    {
        var tasks = _sceneContext.Container.ResolveAll<IInitializableModuleAsync>();
        await UniTask.WhenAll(tasks.Select(i => i.InitializeAsync(cancellationToken))).AttachExternalCancellation(cancellationToken);
    }

    private void SetApplicationFramerate()
    {
        var currentResolutionRefreshRate = Screen.currentResolution.refreshRateRatio;
        var maxRefreshRate = Math.Max((int)Math.Ceiling(currentResolutionRefreshRate.value), DefaultFPS);
        Application.targetFrameRate = maxRefreshRate;
    }

    private void OnDestroy()
    {
        _initializeTokenSource.Cancel();
        _initializeTokenSource.Dispose();
    }
}