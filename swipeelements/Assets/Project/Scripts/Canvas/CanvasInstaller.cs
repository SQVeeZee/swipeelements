using Project.Canvas;
using Project.Core.Utility;
using Project.Gameplay;
using UI;
using UnityEngine;
using Zenject;

public class CanvasInstaller : MonoInstaller
{
    [SerializeField]
    private CanvasItem _canvasItem;
    [SerializeField]
    private BackgroundPanel _backgroundPanel;
    [SerializeField]
    private UIGameSafeAreaPanel _uiSafeArea;

    public override void InstallBindings()
    {
        BindCanvases();
        BindPanels();
    }

    private void BindCanvases() => Container.BindCanvas(_canvasItem, CanvasIds.Background);
    private void BindPanels()
    {
        Container.Bind<BackgroundPanel>().FromInstance(_backgroundPanel).AsSingle();
        Container.BindInterfacesAndSelfTo<UIGameSafeAreaPanel>().FromInstance(_uiSafeArea);
    }
}