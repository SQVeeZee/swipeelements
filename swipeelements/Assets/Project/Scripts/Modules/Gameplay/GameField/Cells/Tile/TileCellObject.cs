using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public class TileCellObject : CellObject
    {
        [SerializeField]
        private TileAnimatorProcessor _animatorProcessor;

        public TileAnimatorProcessor AnimatorProcessor => _animatorProcessor;

        public override async UniTask DestroyCellAsync(CancellationToken cancellationToken)
            => await _animatorProcessor.PlayDestroyAsync(cancellationToken);

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