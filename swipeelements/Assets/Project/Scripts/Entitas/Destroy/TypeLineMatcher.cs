using System.Collections.Generic;

namespace Project.Entitas
{
    internal static class TypeLineMatcher
    {
        public static void BuildIndex(
            IEnumerable<GameEntity> tilesOfType,
            out HashSet<Coord> coords,
            out Dictionary<Coord, GameEntity> byCoord)
        {
            coords = new HashSet<Coord>();
            byCoord = new Dictionary<Coord, GameEntity>();

            foreach (var gameEntity in tilesOfType)
            {
                if (gameEntity == null || !gameEntity.hasTileCoord || !gameEntity.IsDestroyable())
                {
                    continue;
                }

                var c = gameEntity.tileCoord.value;
                coords.Add(c);
                byCoord[c] = gameEntity;
            }
        }

        public static HashSet<Coord> FindLineMatches(HashSet<Coord> coords)
        {
            var marked = new HashSet<Coord>();
            if (coords.Count == 0)
            {
                return marked;
            }

            var visitedH = new HashSet<Coord>();
            var visitedV = new HashSet<Coord>();

            foreach (var c in coords)
            {
                if (!visitedH.Contains(c) && !coords.Contains(new Coord(c.X - 1, c.Y)))
                {
                    var run = ScanRun(coords, c, dx: 1, dy: 0, visitedH);
                    if (run >= 3)
                    {
                        MarkRun(marked, c, dx: 1, dy: 0, length: run);
                    }
                }

                if (!visitedV.Contains(c) && !coords.Contains(new Coord(c.X, c.Y - 1)))
                {
                    var run = ScanRun(coords, c, dx: 0, dy: 1, visitedV);
                    if (run >= 3)
                    {
                        MarkRun(marked, c, dx: 0, dy: 1, length: run);
                    }
                }
            }

            return marked;
        }

        private static int ScanRun(HashSet<Coord> coords, Coord start, int dx, int dy, HashSet<Coord> visited)
        {
            var len = 0;
            var x = start.X;
            var y = start.Y;

            while (coords.Contains(new Coord(x, y)))
            {
                var cur = new Coord(x, y);
                if (!visited.Add(cur))
                {
                    break;
                }

                len++;
                x += dx;
                y += dy;
            }

            return len;
        }

        private static void MarkRun(HashSet<Coord> marked, Coord start, int dx, int dy, int length)
        {
            var x = start.X;
            var y = start.Y;

            for (var i = 0; i < length; i++)
            {
                marked.Add(new Coord(x, y));
                x += dx;
                y += dy;
            }
        }
    }
}
