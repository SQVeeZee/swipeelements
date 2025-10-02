using System.Threading;
using JetBrains.Annotations;
using Project.Core;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class LevelCancellationToken : BaseCancellationToken, ICancellationTokenControl
    {
        public const string Id = "level_cancellation_token";

        CancellationToken ICancellationTokenControl.CreateToken()
        {
            Current = new CancellationTokenSource();
            return Current.Token;
        }

        void ICancellationTokenControl.Cancel()
        {
            Current?.Cancel();
            Current?.Dispose();
            Current = null;
        }
    }
}