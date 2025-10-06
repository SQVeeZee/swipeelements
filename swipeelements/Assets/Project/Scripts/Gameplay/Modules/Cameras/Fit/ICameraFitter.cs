using UnityEngine;

namespace Project.Gameplay
{
    public interface ICameraFitter
    {
        void FitCamera(Bounds bounds);
    }
}