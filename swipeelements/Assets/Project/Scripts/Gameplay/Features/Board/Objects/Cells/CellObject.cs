using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public abstract class CellObject : MonoBehaviour
    {
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        private Action _onDestroy;

        public MergesCell Info { get; protected set; }

        public virtual void Initialize(MergesCell info, Action onDestroy)
        {
            _onDestroy = onDestroy;
            Info = info;
        }

        protected void Destroy() => _onDestroy?.Invoke();

        public virtual void Dispose() { }

        public virtual UniTask DestroyCellAsync(CancellationToken cancellationToken) => default;
        public void SetSortingOrder(int defaultOrder) => _spriteRenderer.sortingOrder = defaultOrder;
    }
}