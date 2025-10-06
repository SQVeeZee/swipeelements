using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public static class MergesStateExtensions
    {
        public static bool IsValid(this MergesState state, (int X, int Y) coord) =>
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

        public static IEnumerable<(int X, int Y)> GetCoords(this MergesState state)
        {
            for (var y = 0; y < state.Rows; y++)
            {
                for (var x = 0; x < state.Columns; x++)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int X, int Y)> GetPlayableCoords(this MergesState state)
        {
            foreach (var (x, y) in state.GetCoords())
            {
                if (!state[x, y].IsVoid)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int X, int Y)> GetTileCoords(this MergesState state)
        {
            foreach (var (x, y) in state.GetCoords())
            {
                if (state[x, y].IsTile)
                {
                    yield return (x, y);
                }
            }
        }
    }
}