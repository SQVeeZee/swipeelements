using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Project.Gameplay.Puzzles
{
    [UsedImplicitly]
    public class MergesGame
    {
        private ILevelData _levelData;
        private MergesState _state;

        public event Action<MergesAction, MergesState, MergesStep> OnGameChanged;

        public void Initialize(MergesState state, ILevelData levelData)
        {
            MergesStep.ResetCounters();
            _state = state;
            _levelData = levelData;

            var dict = _levelData.InitialValues.ToDictionary();
            if (dict != null)
            {
                foreach (var cell in dict)
                {
                    var coord = cell.Key;
                    if (_state[coord].CellType == CellType.None)
                    {
                        _state[coord] = new MergesCell(cell.Value);
                    }
                }
            }

            var gridStep = InitializeGridStep.CalculateStep(_state, levelData);
            ApplyStep(gridStep, MergesAction.None);
        }

        public void ApplySwipe((int X, int Y) from, (int X, int Y) to, HashSet<(int X, int Y)> ignored)
        {
            var moveData = new MoveData(from, to);
            var step = TileActionStep.Create(_state, moveData);
            if (!step.MakeSense)
            {
                return;
            }

            var combineSteps = GetResolveBoardSteps(step.Final, ignored);
            if (combineSteps.Count > 0)
            {
                var combinedSteps = new List<MergesStep>(new[] { step }.Concat(combineSteps));
                var combineStep = CombineStep.CalculateStep(_state, combinedSteps);
                ApplyStep(combineStep, MergesAction.Recordable);
            }
            else
            {
                ApplyStep(step, MergesAction.Recordable);
            }

            TryWinGame(_state);
        }

        public void ResolveBoard(HashSet<(int X, int Y)> ignored)
        {
            var combineSteps = GetResolveBoardSteps(_state, ignored);
            if (combineSteps.Count > 0)
            {
                var combineStep = CombineStep.CalculateStep(_state, combineSteps);
                ApplyStep(combineStep, MergesAction.Recordable);
            }
            TryWinGame(_state);
        }

        private List<MergesStep> GetResolveBoardSteps(MergesState state, HashSet<(int X, int Y)> ignored)
        {
            var steps = new List<MergesStep>();
            var changed = false;
            do
            {
                changed = false;
                tryAddStep(BoardGravityStep.CalculateStep(state, ignored));
                tryAddStep(BoardDestroyStep.CalculateStep(state, ignored));
            } while (changed);

            return steps;

            void tryAddStep(MergesStep step)
            {
                changed |= step.MakeSense;
                if (step.MakeSense)
                {
                    steps.Add(step);
                }
                state = step.Final;
            }
        }

        private void TryWinGame(MergesState state)
        {
            if (state.CountTiles() > 0)
            {
                return;
            }

            var winStep = WinGameStep.CalculateStep(state);
            ApplyStep(winStep, MergesAction.Braking);
        }


        private void ApplyStep(MergesStep step, MergesAction action)
        {
            if (!step.MakeSense)
            {
                return;
            }

            var prev = _state;
            _state = step.Final;
            OnGameChanged?.Invoke(action, prev, step);
        }
    }
}
