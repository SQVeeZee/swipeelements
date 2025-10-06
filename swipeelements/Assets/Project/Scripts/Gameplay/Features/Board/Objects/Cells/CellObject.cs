using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public abstract class CellObject : MonoBehaviour
    {
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        protected CellsContainer CellsContainer { get; private set; }
        protected BoardSettings BoardSettings { get; private set; }

        public MergesCell Info { get; protected set; }

        [Inject]
        private void Construct(
            CellsContainer cellsContainer,
            BoardSettings boardSettings)
        {
            CellsContainer = cellsContainer;
            BoardSettings = boardSettings;
        }

        public virtual void Initialize(MergesCell info) => Info = info;
        public virtual void Dispose() { }

        public virtual UniTask DestroyCellAsync(CancellationToken cancellationToken) => default;
        public void SetSortingOrder(int defaultOrder) => _spriteRenderer.sortingOrder = defaultOrder;
    }
}