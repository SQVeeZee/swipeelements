using JetBrains.Annotations;

namespace Project.Core
{
    [UsedImplicitly]
    public class LevelCancellationToken : BaseCancellationToken
    {
        public const string Id = "level_cancellation_token";
    }
}