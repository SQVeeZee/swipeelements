using System;
using Newtonsoft.Json;

namespace Project.Core
{
    public abstract class ProfileSection : IProfileSection
    {
        public abstract string Key { get; }

        public event Action<ProfileSection> OnChanged;

        public virtual string Serialize()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public virtual void Deserialize(string json) =>
            JsonConvert.PopulateObject(json, this, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });

        public void Save() => OnChanged?.Invoke(this);
    }
}