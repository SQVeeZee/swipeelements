using UnityEngine;

namespace UI
{
    public class UIGameSafeAreaPanel : MonoBehaviour, IUIGameSafeArea
    {
        [SerializeField]
        private RectTransform _bottomUI;

        Vector3 IUIGameSafeArea.GetWorldPositionForField()
        {
            var corners = new Vector3[4];
            _bottomUI.GetWorldCorners(corners);
            return new Vector3(0, corners[1].y, 0);
        }
    }
}