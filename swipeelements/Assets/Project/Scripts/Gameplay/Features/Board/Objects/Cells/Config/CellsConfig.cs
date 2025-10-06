using System;
using System.Collections.Generic;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/cells_config", fileName = "cells_config")]
    public class CellsConfig : ScriptableObject
    {
        [Serializable]
        public struct CellSettings
        {
            [SerializeField]
            private CellType _cellType;
            [SerializeField]
            private CellObject _cellObject;

            public CellType CellType => _cellType;
            public CellObject CellObject => _cellObject;
        }

        [SerializeField]
        private List<CellSettings> _settings = new();

        public int Count => _settings.Count;

        public bool TryGetSettings(int index, out CellSettings result)
        {
            if (index >= _settings.Count)
            {
                result = default;
                return false;
            }
            result = _settings[index];
            return true;
        }

        public bool TryGetSettings(CellType type, out CellSettings result)
        {
            foreach (var settings in _settings)
            {
                if (settings.CellType != type) continue;
                result = settings;
                return true;
            }
            result = default;
            return false;
        }
    }
}