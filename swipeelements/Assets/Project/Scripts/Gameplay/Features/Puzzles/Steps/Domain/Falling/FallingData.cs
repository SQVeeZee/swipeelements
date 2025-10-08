using System.Collections.Generic;
using Project.Entitas;

namespace Project.Gameplay.Puzzles
{
    public struct FallingData
    {
        public MoveData MoveData { get; }
        public HashSet<Coord> Path { get; }

        public FallingData(MoveData moveData, HashSet<Coord> path)
        {
            MoveData = moveData;
            Path = path;
        }
    }
}