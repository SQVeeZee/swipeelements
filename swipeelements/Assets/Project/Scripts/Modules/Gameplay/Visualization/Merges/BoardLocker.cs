using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class BoardLocker
    {
        private readonly Dictionary<string, HashSet<(int X, int Y)>> _lockedCoords = new ();

        public HashSet<(int X, int Y)> GetAllLockedCoords()
        {
            var result = new HashSet<(int X, int Y)>();
            foreach (var set in _lockedCoords.Values)
            {
                result.UnionWith(set);
            }
            return result;
        }

        public void AddLockedCells(string id, HashSet<(int X, int Y)> coords) => _lockedCoords.Add(id, coords);
        public bool Remove(string id) => _lockedCoords.Remove(id);
        public void Clear() => _lockedCoords.Clear();

        public bool ContainsAny(params (int X, int Y)[] coords) => coords.Any(Contains);
        public bool Contains((int X, int Y) coord) => _lockedCoords.Any(lockedCoord => lockedCoord.Value.Contains(coord));
    }
}