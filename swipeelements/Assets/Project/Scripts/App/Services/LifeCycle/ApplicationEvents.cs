using UnityEngine;
using Zenject;

namespace Project.LifeCycle
{
    public class ApplicationEvents : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void OnApplicationQuit() =>
            _signalBus.Fire(new ApplicationQuitSignal());

        private void OnApplicationPause(bool pauseStatus) =>
            _signalBus.Fire(new ApplicationPauseSignal(pauseStatus));

        private void OnApplicationFocus(bool hasFocus) =>
            _signalBus.Fire(new ApplicationFocusSignal(hasFocus));
    }
}