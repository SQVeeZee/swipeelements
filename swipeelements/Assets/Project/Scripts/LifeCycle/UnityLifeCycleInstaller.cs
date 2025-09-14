using UnityEngine;
using Zenject;

namespace Project
{
    public class ApplicationEventsInstaller : MonoInstaller
    {
        [SerializeField]
        private ApplicationEvents _applicationEvents;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ApplicationQuitSignal>();
            Container.DeclareSignal<ApplicationPauseSignal>();
            Container.DeclareSignal<ApplicationFocusSignal>();

            Container.InstantiatePrefabForComponent<ApplicationEvents>(_applicationEvents);
        }
    }
}