using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public struct FallingData
    {
        public MoveData MoveData { get; }
        public HashSet<(int X, int Y)> Path { get; }

        public FallingData(MoveData moveData, HashSet<(int X, int Y)> path)
        {
            MoveData = moveData;
            Path = path;
        }
    }
}