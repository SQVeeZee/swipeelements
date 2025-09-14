using JetBrains.Annotations;
using Project.Gameplay;
using Zenject;

namespace Project.Level
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class GamePoolManager
    {
        private readonly DiContainer _diContainer;
        private readonly CellsFactory _cellsFactory;
        private readonly CellsConfig _cellsConfig;

        public GamePoolManager(
            DiContainer diContainer,
            CellsFactory cellsFactory,
            CellsConfig cellsConfig)
        {
            _diContainer = diContainer;
            _cellsFactory = cellsFactory;
            _cellsConfig = cellsConfig;
        }

        public void BindBoardPools()
        {
            BindCells();
            _cellsFactory.Initialize();
        }

        private void BindCells()
        {
            foreach (var settings in _cellsConfig.Settings)
            {
                var type = settings.CellType;
                _diContainer.Bind<CellObject>().WithId(type).FromInstance(settings.CellObject);
                _diContainer.BindMemoryPool<CellObject, CellsPool>()
                    .WithId(type)
                    .WithInitialSize(20)
                    .FromComponentInNewPrefab(settings.CellObject)
                    .UnderTransformGroup($"Cells-Pool-{type}")
                    .NonLazy();
            }
        }
    }
}