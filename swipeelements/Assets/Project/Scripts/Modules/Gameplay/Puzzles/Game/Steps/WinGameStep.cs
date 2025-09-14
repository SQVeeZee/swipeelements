using UnityEngine;

namespace Project.Gameplay.Puzzles
{
    public class WinGameStep : MergesStep
    {
        public bool IsApply { get; private set; }
        public override bool MakeSense => IsApply;

        public WinGameStep( MergesState initial) : base(initial) { }

        public static WinGameStep CalculateStep(MergesState state)
        {
            var step = new WinGameStep(state)
            {
                IsApply = state.CountTiles() == 0
            };
            return step;
        }
    }
}