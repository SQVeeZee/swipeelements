using UnityEngine;
using Zenject;

namespace Project.LifeCycle
{
    public class ApplicationEventsInstaller : MonoInstaller
    {
        [SerializeField]
        private ApplicationEvents _applicationEvents;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ApplicationQuitSignal>().OptionalSubscriber();
            Container.DeclareSignal<ApplicationPauseSignal>().OptionalSubscriber();
            Container.DeclareSignal<ApplicationFocusSignal>().OptionalSubscriber();

            Container.InstantiatePrefabForComponent<ApplicationEvents>(_applicationEvents);
        }
    }
}