using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;
using Quaternion = UnityEngine.Quaternion;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class CellsFactory : PlaceholderFactory<MergesCell, Vector3, Transform, CellObject>
    {
        private Dictionary<CellType, CellsPool> _pools;
        private CellsConfig _cellsConfig;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(
            CellsConfig cellsConfig,
            DiContainer diContainer)
        {
            _diContainer = diContainer;
            _cellsConfig = cellsConfig;
        }

        public void Initialize()
        {
            _pools = new Dictionary<CellType, CellsPool>(_cellsConfig.Settings.Count);
            foreach (var type in _cellsConfig.Settings.Select(setting => setting.CellType))
            {
                _pools[type] = _diContainer.ResolveId<CellsPool>(type);
            }
        }

        public override CellObject Create(MergesCell cell, Vector3 position, Transform root)
        {
            var pool = _pools[cell.CellType];
            var result = pool.Spawn();
            result.transform.SetParent(root);

            result.transform.SetPositionAndRotation(position, Quaternion.identity);
            return result;
        }

        public void Return(CellObject cell)
        {
            if (!_pools.TryGetValue(cell.Info.CellType, out var pool) || cell == null || cell.gameObject == null)
            {
                return;
            }

            pool.Despawn(cell);
            cell.Dispose();
        }
    }

    [UsedImplicitly]
    public class CellsPool : MonoMemoryPool<CellObject>
    {
    }
}