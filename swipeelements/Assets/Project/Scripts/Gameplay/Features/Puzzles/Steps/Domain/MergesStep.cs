using System;
using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public abstract class MergesStep : PuzzleStep
    {
        private static Dictionary<Type, int> _counters = new();
        public int LocalId { get; }
        public string GlobalId { get; }

        protected MergesStep(MergesState initial) : base(initial)
        {
            var type = GetType();
            _counters.TryAdd(type, 0);

            LocalId = ++_counters[type];
            GlobalId = $"{type.Name}_{LocalId}";
        }

        public static void ResetCounters() => _counters = new Dictionary<Type, int>();
    }
}