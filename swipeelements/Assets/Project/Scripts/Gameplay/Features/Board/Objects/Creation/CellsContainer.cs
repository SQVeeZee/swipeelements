using JetBrains.Annotations;
using Project.Entitas;
using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class CellsContainer
    {
        private CellsFactory _factory;
        private BoardSettings _boardSettings;
        private CellOrderController _cellOrderController;

        [Inject]
        public void Construct(
            CellsFactory factory,
            BoardSettings boardSettings,
            CellOrderController cellOrderController)
        {
            _cellOrderController = cellOrderController;
            _boardSettings = boardSettings;
            _factory = factory;
        }

        public CellObject Spawn(MergesCell cell, SpawnComponent spawnComponent)
        {
            var cellObject = _factory.Create(cell, spawnComponent.position, _boardSettings.CellsRoot);
            _cellOrderController.ApplyCellSortOrder(cellObject, spawnComponent.coord);
            cellObject.Initialize(cell,  () => Return(cellObject));
            return cellObject;
        }

        public void Return(CellObject cellObject) => _factory.Return(cellObject);
    }
}