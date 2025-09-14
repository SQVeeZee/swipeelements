using System;
using Project.Core;

namespace Project.Profile
{
    [Serializable]
    public class GeneralProfile : ProfileSection
    {
        public override string Key => nameof(GeneralProfile);

        public int CurrentLevelIndex { get; set; } = 0;
    }
}