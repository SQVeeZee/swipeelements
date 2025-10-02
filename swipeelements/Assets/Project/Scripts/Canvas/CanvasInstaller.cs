using Project.Canvas;
using Project.Core.Utility;
using UnityEngine;
using Zenject;

public class CanvasInstaller : MonoInstaller
{
    [SerializeField]
    private CanvasItem _canvasItem;

    public override void InstallBindings()
    {
        BindCanvases();
    }

    private void BindCanvases() => Container.BindCanvas(_canvasItem, CanvasIds.Background);
}