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

        public MergesCell Info { get; protected set; }

        public virtual void Initialize(MergesCell info) => Info = info;
        public virtual void Dispose() { }

        public virtual UniTask DestroyCellAsync(CancellationToken cancellationToken) => default;
        public void SetSortingOrder(int defaultOrder) => _spriteRenderer.sortingOrder = defaultOrder;


    }
}