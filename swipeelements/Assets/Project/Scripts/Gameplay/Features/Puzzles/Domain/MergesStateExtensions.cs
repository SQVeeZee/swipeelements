using System.Collections.Generic;
using Project.Entitas;

namespace Project.Gameplay.Puzzles
{
    public static class MergesStateExtensions
    {
        public static bool IsValid(this MergesState state, Coord coord) =>
            coord is { X: >= 0, Y: >= 0 } && coord.X < state.Columns && coord.Y < state.Rows;

        public static int CountTiles(this MergesState state)
        {
            var count = 0;
            for (var y = 0; y < state.Rows; y++)
            {
                for (var x = 0; x < state.Columns; x++)
                {
                    if (state[x, y].IsTile)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static IEnumerable<Coord> GetCoords(this MergesState state)
        {
            for (var y = 0; y < state.Rows; y++)
            {
                for (var x = 0; x < state.Columns; x++)
                {
                    yield return new Coord(x, y);
                }
            }
        }

        public static IEnumerable<Coord> GetPlayableCoords(this MergesState state)
        {
            foreach (var (x, y) in state.GetCoords())
            {
                if (!state[x, y].IsVoid)
                {
                    yield return new Coord(x, y);
                }
            }
        }

        public static IEnumerable<Coord> GetTileCoords(this MergesState state)
        {
            foreach (var (x, y) in state.GetCoords())
            {
                if (state[x, y].IsTile)
                {
                    yield return new Coord(x, y);
                }
            }
        }
    }
}