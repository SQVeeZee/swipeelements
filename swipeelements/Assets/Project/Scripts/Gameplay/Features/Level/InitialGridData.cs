using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    [Serializable]
    public class InitialGridData
    {
        [JsonProperty(PropertyName = "cells")]
        public List<List<CellData>> Cells { get; private set; } = new();

        public Dictionary<(int X, int Y), ICellData> ToDictionary()
        {
            var dict = new Dictionary<(int X, int Y), ICellData>();
            for (var x = 0; x < Cells.Count; x++)
            {
                var cell = Cells[x];
                for (var y = 0; y < cell.Count; y++)
                {
                    dict[(x, y)] = cell[y];
                }
            }
            return dict;
        }

        public int CountOf(CellType type) => Cells.Sum(cells => cells.Count(cell => cell.CellType == type));
    }
}