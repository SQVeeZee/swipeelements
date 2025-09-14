namespace Project.LifeCycle
{
    public class ApplicationQuitSignal
    {
    }

    public class ApplicationPauseSignal
    {
        public bool IsPaused { get; }
        public ApplicationPauseSignal(bool isPaused) => IsPaused = isPaused;
    }

    public class ApplicationFocusSignal
    {
        public bool HasFocus;
        public ApplicationFocusSignal(bool hasFocus) => HasFocus = hasFocus;
    }
}