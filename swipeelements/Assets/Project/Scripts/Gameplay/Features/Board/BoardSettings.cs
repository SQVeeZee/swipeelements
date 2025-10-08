using System;
using System.Collections.Generic;
using Project.Entitas;
using UnityEngine;

namespace Project.Gameplay
{
    public class BoardSettings : MonoBehaviour
    {
        [SerializeField]
        private Transform _cellsRoot;

        private Dictionary<Coord, Vector3> _positions;

        public Transform CellsRoot => _cellsRoot;

        public void Initialize(Dictionary<Coord, Vector3> positions) => _positions = positions;

        public Vector3 GetCellPosition(Coord coord)
        {
            if (_positions.TryGetValue(coord, out var position))
            {
                return position;
            }
            throw new Exception($"Can't find cell position for {coord}");
        }
    }
}