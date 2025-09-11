using System.Collections.Generic;
using System.Linq;

namespace Project.Gameplay.Puzzles
{
    public class CombineStep : MergesStep
    {
        public override bool MakeSense => Steps != null && Steps.Any(c => c.MakeSense);

        public List<MergesStep> Steps { get; private set; }

        public CombineStep(MergesState initial) : base(initial) { }

        public static CombineStep CalculateStep(MergesState state, List<MergesStep> steps)
        {
            var step = new CombineStep(state) { Steps = steps };
            if (step.Steps == null)
            {
                return step;
            }

            var last = step.Steps.LastOrDefault();
            if (last == null)
            {
                return step;
            }

            step.Final = last.Final;
            return step;
        }
    }
}