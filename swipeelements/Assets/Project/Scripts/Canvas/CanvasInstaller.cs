using Project.Canvas;
using Project.Core.Utility;
using Project.Gameplay;
using UnityEngine;
using Zenject;

public class CanvasInstaller : MonoInstaller
{
    [SerializeField]
    private CanvasItem _canvasItem;
    [SerializeField]
    private BackgroundPanel _backgroundPanel;

    public override void InstallBindings()
    {
        BindCanvases();
        BindPanels();
    }

    private void BindCanvases() => Container.BindCanvas(_canvasItem, CanvasIds.Background);
    private void BindPanels() => Container.Bind<BackgroundPanel>().FromInstance(_backgroundPanel).AsSingle();
}