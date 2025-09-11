using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public interface ILockedStep
    {
        HashSet<(int X, int Y)> LockedCoords { get; }
    }
}