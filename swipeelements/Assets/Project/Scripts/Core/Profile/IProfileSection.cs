using System;
using Newtonsoft.Json;

namespace Project.Core
{
    public interface IProfileSection
    {
        [JsonIgnore]
        public string Key { get; }
        public event Action<ProfileSection> OnChanged;
        public string Serialize();
        public void Deserialize(string json);
    }
}