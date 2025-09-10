using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Core;
using UnityEngine;
using Zenject;

namespace Profile
{
    [UsedImplicitly]
    public class ProfileService : Service
    {
        private readonly List<IProfileSection> _sections;

        [Inject]
        private ProfileService(List<IProfileSection> sections) => _sections = sections;

        private static string GetPath(string key) => Path.Combine(Application.persistentDataPath, key + ".json");

        public override UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            LoadAll();
            return UniTask.CompletedTask;
        }

        private void SaveAll()
        {
            foreach (var section in _sections)
            {
                Save(section);
            }
        }

        private void Save(IProfileSection section)
        {
            var path = GetPath(section.Key);
            var json = section.Serialize();
            File.WriteAllText(path, json);
        }

        private void LoadAll()
        {
            foreach (var section in _sections)
            {
                section.OnChanged += Save;

                var path = GetPath(section.Key);
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    section.Deserialize(json);
                }
                else
                {
                    Save(section);
                }
            }
        }

        private static void Save(ProfileSection section)
        {
            var path = GetPath(section.Key);
            var json = section.Serialize();
            File.WriteAllText(path, json);
        }

    }
}