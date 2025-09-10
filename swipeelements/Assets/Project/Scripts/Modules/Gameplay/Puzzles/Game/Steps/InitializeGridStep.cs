using System.Collections.Generic;
using Project.Core.Utility;

namespace Project.Gameplay.Puzzles
{
    public class InitializeGridStep : MergesStep
    {
        public ILevelData Level;
        public override bool MakeSense => Level != null;
        public HashSet<(int x, int y)> Spawned { get; set; }

        public bool IsInitialize { get; set; }

        public InitializeGridStep(MergesState initial) : base(initial) { }

        public static InitializeGridStep CalculateStep(MergesState state, ILevelData levelData)
        {
            var step = new InitializeGridStep(state)
            {
                Spawned = new HashSet<(int x, int y)>(),
                Level = levelData,
            };

            FillGrid(step);
            step.IsInitialize = true;

            return step;
        }

        private static void FillGrid(InitializeGridStep step)
        {
            var coords = step.Final.GetPlayableCoords();
            step.Spawned.AddRange(coords);
        }
    }
}