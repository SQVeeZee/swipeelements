using System.Collections.Generic;
using Project.Entitas;

namespace Project.Gameplay.Puzzles
{
    public class InitializeGridStep : MergesStep
    {
        public override bool MakeSense => Spawned.Count > 0;
        public HashSet<Coord> Spawned { get; set; }

        public InitializeGridStep(MergesState initial) : base(initial) { }

        public static InitializeGridStep CalculateStep(MergesState state)
        {
            var step = new InitializeGridStep(state)
            {
                Spawned = new HashSet<Coord>()
            };

            FillGrid(step);
            return step;
        }

        private static void FillGrid(InitializeGridStep step)
        {
            var coords = step.Final.GetPlayableCoords();
            foreach (var coord in coords)
            {
                var cell = step.Initial[coord];
                var cellType = cell.CellType;
                var cellState = cellType.IsTile() ? CellState.Idle : CellState.None;
                step.Final[coord] = cell.ChangeCell(cellState);
                step.Spawned.Add(coord);
            }
        }
    }
}