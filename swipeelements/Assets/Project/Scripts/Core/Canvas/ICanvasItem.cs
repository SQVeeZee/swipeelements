using UnityEngine;

namespace Project.Core
{
    public interface ICanvasItem
    {
        Canvas Canvas { get; }
        RectTransform RectTransform { get; }
    }
}