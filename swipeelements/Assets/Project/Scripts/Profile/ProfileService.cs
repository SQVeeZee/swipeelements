using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Core;
using Project.LifeCycle;
using UnityEngine;
using Zenject;

namespace Project.Profile
{
    [UsedImplicitly]
    public class ProfileService : Service, ITickable
    {
        private readonly List<IProfileSection> _sections;
        private readonly SignalBus _signalBus;

        private readonly HashSet<IProfileSection> _dirtySections = new();

        [Inject]
        private ProfileService(List<IProfileSection> sections, SignalBus signalBus)
        {
            _sections = sections;
            _signalBus = signalBus;
        }

        private static string GetPath(string key) => Path.Combine(Application.persistentDataPath, key + ".json");

        protected override UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            LoadAll();
            Subscribe();
            return UniTask.CompletedTask;
        }

        private void Subscribe()
        {
            foreach (var section in _sections)
            {
                section.OnChanged += MarkDirty;
            }

            _signalBus.Subscribe<ApplicationQuitSignal>(OnQuit);
            _signalBus.Subscribe<ApplicationPauseSignal>(OnPause);
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var section in _sections)
            {
                section.OnChanged -= MarkDirty;
            }

            _signalBus.Unsubscribe<ApplicationQuitSignal>(OnQuit);
            _signalBus.Unsubscribe<ApplicationPauseSignal>(OnPause);
        }

        void ITickable.Tick()
        {
            if (_dirtySections.Count == 0)
            {
                return;
            }

            foreach (var section in _dirtySections)
            {
                Save(section);
            }

            _dirtySections.Clear();
        }

        private void MarkDirty(ProfileSection section) => _dirtySections.Add(section);

        private void OnQuit() => SaveAll();

        private void OnPause(ApplicationPauseSignal signal)
        {
            if (signal.IsPaused)
                SaveAll();
        }

        private void SaveAll()
        {
            foreach (var section in _sections)
            {
                Save(section);
            }

            _dirtySections.Clear();
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
    }
}