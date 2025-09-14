namespace Project
{
    public class ApplicationQuitSignal
    {
    }

    public class ApplicationPauseSignal
    {
        public bool IsPaused;
        public ApplicationPauseSignal(bool isPaused) => IsPaused = isPaused;
    }

    public class ApplicationFocusSignal
    {
        public bool HasFocus;
        public ApplicationFocusSignal(bool hasFocus) => HasFocus = hasFocus;
    }
}