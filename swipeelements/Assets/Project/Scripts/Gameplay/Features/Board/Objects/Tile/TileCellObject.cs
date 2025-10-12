

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Entitas;
using Entitas.Unity;
using Project.Core;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class TileCellObject : CellObject, ITileView, ITilePositionListener, ITileSortOrderListener, IDestroyTileListener
    {
        [SerializeField]
        private TileAnimatorProcessor _animatorProcessor;
        private ITilePositionListener _iPositionListenerImplementation;
        private ICancellationToken _cancellationToken;

        public TileAnimatorProcessor AnimatorProcessor => _animatorProcessor;

        public GameEntity Entity { get; private set; }

        [Inject]
        private void Construct([Inject(Id = ModuleCancellationToken.Id)] ICancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public void Link(IEntity entity)
        {
            gameObject.Link(entity);
            Entity = (GameEntity)entity;
            Entity.AddTilePositionListener(this);
            Entity.AddTileSortOrderListener(this);
            Entity.AddDestroyTileListener(this);
        }

        void ITilePositionListener.OnTilePosition(GameEntity entity, Vector3 value) => transform.position = value;
        void ITileSortOrderListener.OnTileSortOrder(GameEntity entity, int value) => SetSortingOrder(value);
        void IDestroyTileListener.OnDestroyTile(GameEntity entity) => DestroyCellAsync(_cancellationToken.Token).Forget();

        public override async UniTask DestroyCellAsync(CancellationToken cancellationToken)
        {
            Info.ChangeCell(CellState.Destroyed);
            await _animatorProcessor.PlayDestroyAsync(cancellationToken);
            Destroy();
        }

        public override void Initialize(MergesCell info, Action onDestroy)
        {
            base.Initialize(info, onDestroy);
            _animatorProcessor.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
            _animatorProcessor.Dispose();
        }
    }
}