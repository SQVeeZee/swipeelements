using System.Threading;
using Cysharp.Threading.Tasks;
using Entitas;
using Entitas.Unity;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public class TileCellObject : CellObject, ITileView, ITilePositionListener, ITileSortOrderListener
    {
        [SerializeField]
        private TileAnimatorProcessor _animatorProcessor;
        private ITilePositionListener _iPositionListenerImplementation;

        public TileAnimatorProcessor AnimatorProcessor => _animatorProcessor;

        public GameEntity Entity { get; private set; }

        public void Link(IEntity entity)
        {
            gameObject.Link(entity);
            Entity = (GameEntity)entity;
            Entity.AddTilePositionListener(this);
            Entity.AddTileSortOrderListener(this);
        }

        void ITilePositionListener.OnTilePosition(GameEntity entity, Vector3 value) => transform.position = value;
        void ITileSortOrderListener.OnTileSortOrder(GameEntity entity, int value) => SetSortingOrder(value);

        public override async UniTask DestroyCellAsync(CancellationToken cancellationToken)
        {
            Info.ChangeCell(CellState.Destroyed);
            await _animatorProcessor.PlayDestroyAsync(cancellationToken);
        }

        public override void Initialize(MergesCell info)
        {
            base.Initialize(info);
            _animatorProcessor.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
            _animatorProcessor.Dispose();
        }

    }
}