using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class InitializeGridVisualizeStep: VisualizeStep<InitializeGridStep>
    {
        public InitializeGridVisualizeStep(
            StepsVisualizer visualizer,
            InitializeGridStep step)
            : base(visualizer, step) { }

        public override UniTask ApplyAsync(CancellationToken cancellationToken)
        {
            SpawnCells();
            return UniTask.CompletedTask;
        }


        private void SpawnCells()
        {
            foreach (var coord in Step.Final.GetPlayableCoords())
            {
                if (!Step.Final[coord].IsRegular)
                {
                    continue;
                }

                SpawnCell(coord);
            }
        }

        private void SpawnCell((int X, int Y) coord) => CellsContainer.Spawn(Step.Final[coord], coord);
    }
}