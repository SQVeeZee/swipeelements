using Zenject;

namespace Project.Gameplay
{
    public class VisualizationProgress
    {
        private readonly StepsVisualizer _stepsVisualizer;

        [Inject]
        private VisualizationProgress(StepsVisualizer stepsVisualizer) => _stepsVisualizer = stepsVisualizer;

        public bool IsVisualizing => _stepsVisualizer.IsVisualizing;
    }
}