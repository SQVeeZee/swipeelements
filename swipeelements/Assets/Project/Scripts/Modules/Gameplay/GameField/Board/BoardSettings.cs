using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay
{
    public class BoardSettings : MonoBehaviour
    {
        [SerializeField]
        private Transform _cellsRoot;

        private Dictionary<(int X, int Y), Vector3> _positions;

        public Transform CellsRoot => _cellsRoot;

        public void Initialize(Dictionary<(int X, int Y), Vector3> positions) => _positions = positions;

        public Vector3 GetCellPosition((int X, int Y) coord)
        {
            if (_positions.TryGetValue(coord, out var position))
            {
                return position;
            }
            throw new Exception($"Can't find cell position for {coord}");
        }
    }
}