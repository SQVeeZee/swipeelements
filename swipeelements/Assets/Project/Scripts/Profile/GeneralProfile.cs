using System;
using Project.Core;

namespace Profile
{
    [Serializable]
    public class GeneralProfile : ProfileSection
    {
        public override string Key => nameof(GeneralProfile);

        public int CurrentLevelIndex { get; set; } = 0;
    }
}