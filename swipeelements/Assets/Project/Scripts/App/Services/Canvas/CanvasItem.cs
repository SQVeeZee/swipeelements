using Project.Core;
using UnityEngine;

public class CanvasItem : MonoBehaviour, ICanvasItem
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private RectTransform _rectTransform;

    public Canvas Canvas => _canvas;
    public RectTransform RectTransform => _rectTransform;

    private void OnValidate()
    {
        if (_canvas == null)
        {
            _canvas = GetComponent<Canvas>();
        }
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}