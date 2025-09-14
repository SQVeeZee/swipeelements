using JetBrains.Annotations;
using Project.Core;
using Project.Gameplay.Puzzles;

namespace Project.Profile
{
    [UsedImplicitly]
    public class SessionProfile : ProfileSection
    {
        public MergesState MergesState { get; set; }
        public override string Key => nameof(SessionProfile);

        public void Clear() => MergesState = null;
    }
}