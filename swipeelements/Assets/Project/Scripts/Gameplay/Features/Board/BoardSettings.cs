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
    }
}