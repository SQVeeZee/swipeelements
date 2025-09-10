using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Gameplay
{
    public class TileAnimatorProcessor : MonoBehaviour
    {
        private static readonly int _idle = Animator.StringToHash("idle");
        private static readonly int _destroy = Animator.StringToHash("destroy");

        private const string DestroyOverride = "tile_destroy";

        [SerializeField]
        private Animator _animator;

        public void Initialize()
        {
            _animator.Play(0, 0, 0f);
            _animator.Update(0f);
        }

        public void Dispose()
        {
            _animator.ResetTrigger(_idle);
            _animator.ResetTrigger(_destroy);
        }

        public void PlayIdle() => _animator.SetTrigger(_idle);

        public async UniTask PlayDestroyAsync(CancellationToken cancellationToken)
        {
            _animator.SetTrigger(_destroy);

            var controller = _animator.runtimeAnimatorController as AnimatorOverrideController;
            if (controller == null)
            {
                throw new InvalidOperationException("Animator is not using AnimatorOverrideController");
            }

            var destroyClip = controller[DestroyOverride];
            var length = destroyClip.length;

            await UniTask.Delay(TimeSpan.FromSeconds(length), cancellationToken: cancellationToken);
        }
    }
}